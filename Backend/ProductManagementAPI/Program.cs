using Microsoft.Azure.Cosmos;
using Azure.Storage.Files.Shares;
using ProductManagementAPI.Services;
using ProductManagementAPI.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductManagementAPI",
        Version = "v1"
    });
});

var cosmosDbConfig = builder.Configuration.GetSection("CosmosDb");
var endpointUri = cosmosDbConfig["EndpointUri"];
var primaryKey = cosmosDbConfig["PrimaryKey"];
var databaseName = cosmosDbConfig["DatabaseName"];
var containerName = cosmosDbConfig["ContainerName"];

var cosmosClient = new CosmosClient(endpointUri, primaryKey);
var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
await database.Database.CreateContainerIfNotExistsAsync(containerName, "/Category");

builder.Services.AddSingleton<CosmosClient>(cosmosClient);

var fileStorageConfig = builder.Configuration.GetSection("AzureFileStorage");
var connectionString = fileStorageConfig["ConnectionString"];
builder.Services.AddSingleton<ShareServiceClient>(sp =>
{
    return new ShareServiceClient(connectionString);
});

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseMiddleware<ProductManagementAPI.Middlewares.ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductManagementAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

// Static files aur Fallback ko Controllers se pehle lagana zaroori hai
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
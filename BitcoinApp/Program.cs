// Import necessary namespaces for setting up the app and services
using BitcoinApp.Services.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
//using BitcoinApp.Services.External;

var builder = WebApplication.CreateBuilder(args);  // Creates a builder for your web application

// Add services to the container (dependency injection)
builder.Services.AddControllersWithViews();  // Enables MVC controllers and views support

// Add API versioning for the API endpoints
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);  // Default version for APIs
    options.AssumeDefaultVersionWhenUnspecified = true;  // Automatically use the default version if no version is specified
    options.ReportApiVersions = true;  // Make API version information available in response headers
});

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BitcoinApp", Version = "v1" });  // Setup the API documentation for version 1

    // Enable XML comments for better documentation, useful for API descriptions
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);  // Include XML comments in Swagger UI
});

// Configure the database connection using the connection string in appsettings.json
// You provided the connection string in appsettings.json, so we will just reference it directly.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register a custom service to interact with your database using ADO.NET
// This service will perform database operations instead of using Entity Framework.
builder.Services.AddScoped<ITransactionService, TransactionService>(provider =>
    new TransactionService(connectionString));  // Passing the connection string to the service

var app = builder.Build();  // Build the web application

// Configure the HTTP request pipeline (define how requests are handled)
if (!app.Environment.IsDevelopment())  // Only use the following for non-development environments (like production)
{
    app.UseExceptionHandler("/Home/Error");  // Use an error handler for production
    app.UseHsts();  // HTTP Strict Transport Security (force HTTPS)
}

app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();  // Enable serving static files (like images, CSS, JS)

app.UseRouting();  // Set up routing to map HTTP requests to controllers

app.UseAuthorization();  // Enable authorization for routes (checking if the user is allowed)

// This ensures that your API controllers are correctly mapped and routed
app.MapControllers();

// Set up Swagger middleware (only in development environment by default)
app.UseSwagger();  // Enable the Swagger JSON generation endpoint
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BitcoinApp v1");  // Point to the Swagger JSON file for the UI
    options.RoutePrefix = string.Empty;  // Set Swagger UI to be at the root URL
});

app.Run();  // Start the application and handle incoming HTTP requests

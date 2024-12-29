using BitcoinApp.Services.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (dependency injection)
builder.Services.AddControllers(); // Only need this if you're using API controllers, not MVC

// Add API versioning for the API endpoints
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments (ensure XML documentation file is generated in project properties)
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BitcoinApp", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.EnableAnnotations(); // Enable annotations for Swagger
});


// Configure the database connection using the connection string in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add Restful Services
builder.Services.AddScoped<ITransactionService, TransactionService>(provider =>
    new TransactionService(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BitcoinApp v1");
        options.RoutePrefix = string.Empty; // Swagger UI at root
    });
}
else
{
    // Use custom error handling in production
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

app.Run();

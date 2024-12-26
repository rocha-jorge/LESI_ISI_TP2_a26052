// Import necessary namespaces for setting up the app and services
using BitcoinApp.Services.Internal;
using BitcoinApp.Services.SOAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using CoreWCF;
using CoreWCF.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (dependency injection)
builder.Services.AddControllersWithViews();

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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BitcoinApp", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Configure the database connection using the connection string in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<ITransactionService, TransactionService>(provider =>
    new TransactionService(connectionString));

// Add services to the container (dependency injection)
builder.Services.AddControllersWithViews();

// Register CoreWCF for SOAP support
builder.Services.AddCoreWCF().AddServiceModelServices();  // Ensure CoreWCF is added

var app = builder.Build();

// Configure the HTTP request pipeline (define how requests are handled)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BitcoinApp v1");
    options.RoutePrefix = string.Empty;
});

// Map the SOAP service
app.UseServiceModel(builder =>
{
    builder.AddService<TransactionSoapService>()
           .AddServiceEndpoint<TransactionSoapService, ITransactionSoapService>(new BasicHttpBinding(), "/TransactionService");
});

app.Run();

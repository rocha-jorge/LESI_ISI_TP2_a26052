using BitcoinApp.Auxiliar;
using BitcoinApp.Models;
using BitcoinApp.Services;
using BitcoinApp.Services.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

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

builder.Services.AddSwaggerGen(options =>
{
    // Swagger document settings
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BitcoinApp", Version = "v1" });

    // Add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };
    options.AddSecurityRequirement(securityRequirement);

    // Enable XML comments (ensure XML documentation file is generated in project properties)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Enable annotations for Swagger
    options.EnableAnnotations();
});


// Configure the database connection using the connection string in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add Restful Services
builder.Services.AddScoped<ITransactionService, TransactionService>(provider =>
    new TransactionService(connectionString));

// Configurar autenticação JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(); // Add default authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("Guest", policy => policy.RequireRole("guest"));
    options.AddPolicy("AdminOrGuest", policy =>
                                policy.RequireAssertion(context =>
    context.User.IsInRole("admin") || context.User.IsInRole("guest")));
});

builder.Services.AddTransient<AuthService>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BitcoinApp v1");
    options.RoutePrefix = string.Empty; // Swagger UI at root
});

// Enable production-specific error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();

#region Maping Minimal Endpoints

app.MapPost("/Authenticate", (UserAuth userAuth, AuthService authService)
    => authService.GenerateToken(userAuth));

app.MapGet("/Signin", () => "User Authenticated Successfully!")
    .RequireAuthorization("Admin");

#endregion

app.Run();

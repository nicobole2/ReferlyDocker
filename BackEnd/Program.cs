using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure CORS
builder.Services.AddCors(options => {
    options.AddPolicy("DevCors", corsBuilder => {
        corsBuilder.WithOrigins(
            "http://localhost:4200",
            "http://localhost:3000",
            "http://localhost:8000",
            "http://localhost:5173",
            "http://uilg-1457697790.sa-east-1.elb.amazonaws.com",
            "http://netlb-241207186.sa-east-1.elb.amazonaws.com:5000"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });

    options.AddPolicy("ProdCors", corsBuilder => {
        corsBuilder.WithOrigins(
            "https://productionSite.com",
            "http://uilg-1457697790.sa-east-1.elb.amazonaws.com",
            "http://netlb-241207186.sa-east-1.elb.amazonaws.com:5000"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// Add controllers and JSON configuration
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
}

// Configure JWT Authentication
string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;
var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString ?? ""));
var tokenValidationParameters = new TokenValidationParameters {
    IssuerSigningKey = symmetricSecurityKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = true,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = tokenValidationParameters;
    });

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

// Middleware for handling preflight (OPTIONS) requests explicitly
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.StatusCode = 204; // No Content
        return;
    }
    await next();
});

// Global error handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(error.Error, "An unhandled exception occurred.");

            await context.Response.WriteAsJsonAsync(new {
                error = "An error occurred processing your request.",
                details = app.Environment.IsDevelopment() ? error.Error.Message : null
            });
        }
    });
});

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Endpoint for health checks
app.MapGet("/chequeo", (ILogger<Program> logger) => {
    logger.LogInformation("Health check endpoint hit");
    return Results.Ok(new { status = "Healthy" });
}).WithName("CustomHealthCheck").AllowAnonymous();

// Map controllers
app.MapControllers();

// Run the application
app.Run();

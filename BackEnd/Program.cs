using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add CORS configuration first
builder.Services.AddCors(options => {
    options.AddPolicy("DevCors", corsBuilder => {
        corsBuilder.WithOrigins(
            "http://localhost:4200",
            "http://localhost:3000",
            "http://localhost:8000",
            "http://localhost:5173",
            "http://netlb-241207186.sa-east-1.elb.amazonaws.com:5000"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
    options.AddPolicy("ProdCors", corsBuilder => {
        corsBuilder.WithOrigins("https://productonSite.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
}

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

SymmetricSecurityKey symmetricSecurityKey = new(
    Encoding.UTF8.GetBytes(tokenKeyString ?? "")
);

TokenValidationParameters tokenValidationParameters = new() {
    IssuerSigningKey = symmetricSecurityKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = tokenValidationParameters;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
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


app.MapGet("/chequeo", (ILogger<Program> logger) => {
    logger.LogInformation("Health check endpoint hit");
    return Results.Ok(new { status = "Healthy" });
}).WithName("CustomHealthCheck")
  .AllowAnonymous();
  

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

// Add authentication after CORS
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
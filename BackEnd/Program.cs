using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks(); // Registra Health Checks

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
            "http://localhost:5173"
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

var app = builder.Build();
   
//app.MapHealthChecks("/health").WithName("HealthCheck");

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


app.MapGet("/health", (ILogger<Program> logger) => {
    return Results.Ok(new { status = "Healthy" });
}).WithName("HealthCheck")
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
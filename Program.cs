using DmatAccountApi.Middleware;
using DmatAccountApi.Logging;
using Serilog;

// var builder = WebApplication.CreateBuilder(args);

// // Configure custom time format for Serilog and make sure it's in IST (UTC +5:30)
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Information() // Set minimum log level to Information
//     .WriteTo.Console(outputTemplate: "{Timestamp:dd-MM-yyyy hh:mm:ss tt} | {Level:u3} | {Message} {NewLine}{Exception}") // Custom timestamp format for console
//     .WriteTo.File(
//         path: $"Logs/{DateTime.UtcNow.AddHours(5.5):dd-MM-yyyy}-Request-Response.log", // Log file name with IST date
//         fileSizeLimitBytes: 5 * 1024 * 1024, // Rotate logs if file size exceeds 5MB
//         rollOnFileSizeLimit: true, // Create a new file when size limit is reached
//         outputTemplate: "{Timestamp:dd-MM-yyyy hh:mm:ss tt} | {Level:u3} | {Message} {NewLine}{Exception}" // Custom timestamp format for log file
//     )
//     .CreateLogger();

// // Set Serilog as the logging provider
// builder.Logging.ClearProviders(); // Clear default loggers
// builder.Logging.AddSerilog(); // Add Serilog for logging

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Register custom validation middleware before other middleware that processes the request
// app.UseMiddleware<ValidationMiddleware>();

// app.UseSwagger();
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//     c.RoutePrefix = string.Empty; // This makes Swagger UI available at the root URL
// });

// app.UseMiddleware<RequestResponseLoggingMiddleware>();

// app.MapControllers();

// app.Run();









// using DmatAccountApi.Middleware;
// using DmatAccountApi.Logging;
// using Serilog;

// var builder = WebApplication.CreateBuilder(args);

// // Configure custom time format for Serilog and make sure it's in IST (UTC +5:30)
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Information() // Set minimum log level to Information
//     .WriteTo.Console(outputTemplate: "{Timestamp:dd-MM-yyyy hh:mm:ss tt} | {Level:u3} | {Message} {NewLine}{Exception}") // Custom timestamp format for console
//     .WriteTo.File(
//         path: $"Logs/{DateTime.UtcNow.AddHours(5.5):dd-MM-yyyy}-Request-Response.log", // Log file name with IST date
//         fileSizeLimitBytes: 5 * 1024 * 1024, // Rotate logs if file size exceeds 5MB
//         rollOnFileSizeLimit: true, // Create a new file when size limit is reached
//         outputTemplate: "{Timestamp:dd-MM-yyyy hh:mm:ss tt} | {Level:u3} | {Message} {NewLine}{Exception}" // Custom timestamp format for log file
//     )
//     .CreateLogger();

// // Set Serilog as the logging provider
// builder.Logging.ClearProviders(); // Clear default loggers
// builder.Logging.AddSerilog(); // Add Serilog for logging

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// app.UseSwagger();

// // Set the Swagger UI to be served at the application's root URL
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//     c.RoutePrefix = string.Empty; // This makes Swagger UI available at the root URL
// });

// // Register custom validation middleware
// app.UseMiddleware<RequestResponseLoggingMiddleware>();
// app.UseMiddleware<ValidationMiddleware>();

// app.MapControllers();

// app.Run();


using DmatAccountApi.Logging;
using Microsoft.OpenApi.Models;
using DmatAccountApi.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure custom time format for Serilog and make sure it's in IST (UTC +5:30)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Set minimum log level to Information
    .WriteTo.Console(outputTemplate: "{Timestamp:dd-MM-yyyy hh:mm:ss tt} | {Level:u3} | {Message} {NewLine}{Exception}") // Custom timestamp format for console
    .WriteTo.File(
        path: $"Logs/{DateTime.UtcNow.AddHours(5.5):dd-MM-yyyy}-Request-Response.log", // Log file name with IST date
        fileSizeLimitBytes: 5 * 1024 * 1024, // Rotate logs if file size exceeds 5MB
        rollOnFileSizeLimit: true, // Create a new file when size limit is reached
        outputTemplate: "{Timestamp:dd-MM-yyyy hh:mm:ss tt} | {Level:u3} | {Message} {NewLine}{Exception}" // Custom timestamp format for log file
    )
    .CreateLogger();

// Set Serilog as the logging provider
builder.Logging.ClearProviders(); // Clear default loggers
builder.Logging.AddSerilog(); // Add Serilog for logging

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "A sample API for testing"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "API Documentation";
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

// Use the custom request/response logging middleware
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.Run();
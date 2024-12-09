using DmatAccountApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Register custom validation middleware
app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();

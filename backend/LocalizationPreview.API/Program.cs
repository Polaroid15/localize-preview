using System.Reflection;
using LocalizationPreview.API.Middleware;
using LocalizationPreview.Core.Interfaces;
using LocalizationPreview.Infrastructure.Extensions;
using LocalizationPreview.Infrastructure.Logging;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLoggerAdapter<>));
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Start localization preview app");
app.Run();
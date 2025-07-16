using Bison.UserWebservice.Application;
using Bison.UserWebservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();

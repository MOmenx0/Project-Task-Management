using ProjectTaskManagement.Application;
using ProjectTaskManagement.Infrastructure;
using ProjectTaskManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Api;
using ProjectTaskManagement.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastrctureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddApiDocumentation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseApiDocumentation(); // Swagger UI reads /openapi/v1.json
}

app.Run();

public partial class Program;

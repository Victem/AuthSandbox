using Microsoft.AspNetCore.Mvc;

using System.Net.WebSockets;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/test", (context) =>
{
    //context.Request.HttpContext.User
    //var result = new ViewResult();
    //var actionContext = app.Services.GetRequiredService<ActionContext>();
    return Task.FromResult(Results.Ok(""));
});

app.UseAuthorization();

app.MapControllers();

app.Run();

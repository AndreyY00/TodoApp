using TodoApp.API.Middleware;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Repositories;
using TodoApp.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();


//app.UseSwagger();
//app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();  

app.UseCors("AllowAngular");              

app.UseAuthorization();                   

app.MapControllers();                     
// ─────────────────────────────────────────────

app.Run();
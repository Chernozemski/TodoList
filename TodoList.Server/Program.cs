using Microsoft.EntityFrameworkCore;
using TodoList.Server.Base.Models;
using TodoList.Server.Todos.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddDbContextPool<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")))
	.AddScoped<TodosService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

Endpoints.Map(app);
await MigrationRunner.Run(app.Services);

await app.RunAsync();
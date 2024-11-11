using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TodoList.Server.Todos.Models;
using TodoList.Server.Todos.Services;

namespace TodoList.Server.Base.Models;

public static class Endpoints
{
	public static void Map(WebApplication app)
	{
		MapTodos(app);
	}

	private static void MapTodos(WebApplication app)
	{
		var todos = app.MapGroup("/todos");
		todos.MapGet("/", (TodosService todosService) => todosService.GetTodos());

		todos.MapPost("/", async (TodosService todosService, [FromBody] CreateTodoModel model) =>
			AttributeValidations.Validate(model, out var errors)
			? Results.Ok(await todosService.CreateTodo(model))
			: Results.BadRequest(errors));

		todos.MapPut("/{id}", async (TodosService todosService, [Range(0, int.MaxValue)] int id, [FromBody] UpdateTodoModel model) =>
		{
			if (!AttributeValidations.Validate(model, out var errors))
				return Results.BadRequest(errors);

			await todosService.UpdateTodo(model with { Id = id });
			return Results.Ok();
		});

		todos.MapPatch("/{id}/done", (TodosService todosService, [Range(0, int.MaxValue)] int id) => todosService.MarkAsDone(id));
	}
}
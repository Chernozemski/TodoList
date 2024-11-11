using System.ComponentModel.DataAnnotations;

namespace TodoList.Server.Todos.Models;

public record CreateTodoModel(
	[property: Required, StringLength(200, MinimumLength = 2)]
	string Title,
	DateTime? DueDate
);
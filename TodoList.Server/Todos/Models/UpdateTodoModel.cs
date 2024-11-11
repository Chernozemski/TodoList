using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoList.Server.Todos.Models;

public record UpdateTodoModel(
	[property: JsonIgnore]
	int Id,
	[property: Required, StringLength(200, MinimumLength = 2)]
	string Title,
	DateTime? DueDate
);
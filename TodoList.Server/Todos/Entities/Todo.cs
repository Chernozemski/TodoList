using System.ComponentModel.DataAnnotations;

namespace TodoList.Server.Todos.Entities;

public class Todo
{
    public int Id { get; set; }

	[Required]
	public string Title { get; set; } = null!;

	public DateTime? DueDate { get; set; }

	public bool IsCompleted { get; set; }

	public Todo(int id, string title, DateTime? dueDate, bool isCompleted)
	{
		Id = id;
		Title = title;
		DueDate = dueDate;
		IsCompleted = isCompleted;
	}

	public Todo(string title, DateTime? dueDate)
	{
		Title = title;
		DueDate = dueDate;
	}
}
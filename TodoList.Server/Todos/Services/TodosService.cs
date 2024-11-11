using Microsoft.EntityFrameworkCore;
using TodoList.Server.Base.Models;
using TodoList.Server.Todos.Models;

namespace TodoList.Server.Todos.Services;

public class TodosService(AppDbContext _appDbContext)
{
	public Task<TodoDetails[]> GetTodos() =>
		_appDbContext.Todos
		.Where(t => !t.IsCompleted)
		.OrderByDescending(d => d.DueDate != null)
			.ThenBy(d => d.DueDate)
		.Select(t => new TodoDetails(t.Id, t.Title, t.DueDate))
		.ToArrayAsync();


	public async Task<int> CreateTodo(CreateTodoModel model)
	{
		var todo = _appDbContext.Todos.Add(new(model.Title, model.DueDate)).Entity;

		await _appDbContext.SaveChangesAsync();

		return todo.Id;
	}

	public async Task UpdateTodo(UpdateTodoModel model)
	{
		var todo = await _appDbContext.Todos.Where(t => t.Id == model.Id).FirstOrDefaultAsync()
			?? throw new ArgumentException("Todo not found", nameof(model.Id));

		if (todo.IsCompleted)
			throw new ArgumentException("Todo is completed");

		todo.Title = model.Title;
		todo.DueDate = model.DueDate;

		await _appDbContext.SaveChangesAsync();
	}

	public async Task MarkAsDone(int todoId)
	{
		var todo = await _appDbContext.Todos.Where(t => t.Id == todoId).FirstOrDefaultAsync()
			?? throw new ArgumentException("Todo not found", nameof(todoId));

		todo.IsCompleted = true;

		await _appDbContext.SaveChangesAsync();
	}
}
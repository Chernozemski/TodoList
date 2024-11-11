using Microsoft.EntityFrameworkCore;
using TodoList.Server.Base.Models;
using TodoList.Server.Todos.Entities;
using TodoList.Server.Todos.Models;
using TodoList.Server.Todos.Services;

namespace TodoList.Tests;

public class TodosServiceTests : IDisposable
{
	private readonly AppDbContext _appDbContext;

	public TodosServiceTests() =>
		_appDbContext = new(new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(databaseName: "todolist")
			.Options);

	public void Dispose() =>
		_appDbContext.Database.EnsureDeleted();

	[Fact]
	public async Task GetTodos_ReturnsUncompletedTodosOrderedByDueDate()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);

		var todo1 = new Todo(1, "furthest", DateTime.Now.AddDays(2), false);
		var todo2 = new Todo(2, "closest", DateTime.Now.AddDays(1), false);
		var todo3 = new Todo(3, "no-date-completed", null, true);
		var todo4 = new Todo(4, "no-date", null, false);

		await _appDbContext.AddRangeAsync(todo1, todo2, todo3, todo4);
		await _appDbContext.SaveChangesAsync();

		// Act
		var result = await todosService.GetTodos();

		// Assert
		Assert.Equal(3, result.Length);
		Assert.Equal("closest", result[0].Title);
		Assert.Equal("furthest", result[1].Title);
		Assert.Equal("no-date", result[2].Title);
	}

	[Fact]
	public async Task CreateTodo_AddsNewTodoToDatabase()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);
		DateTime dueDate = DateTime.Now.AddDays(1);
		var model = new CreateTodoModel("Test Todo", dueDate);

		// Act
		var id = await todosService.CreateTodo(model);

		// Assert
		Assert.Equal(1, id);
		Assert.Equal(1, _appDbContext.Todos.Count());
		var todo = _appDbContext.Todos.First();
		Assert.Equal("Test Todo", todo.Title);
		Assert.Equal(dueDate, todo.DueDate);
	}

	[Fact]
	public async Task UpdateTodo_UpdatesExistingTodoInDatabase()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);

		var dueDate = DateTime.Now;
		var todo = new Todo(1, "Test Todo", dueDate, false);
		await _appDbContext.AddAsync(todo);
		await _appDbContext.SaveChangesAsync();

		var diffDueDate = dueDate.AddDays(2);
		var model = new UpdateTodoModel(1, "Updated Todo", diffDueDate);

		// Act
		await todosService.UpdateTodo(model);

		// Assert
		Assert.Equal(1, _appDbContext.Todos.Count());
		var updatedTodo = _appDbContext.Todos.First();
		Assert.Equal("Updated Todo", updatedTodo.Title);
		Assert.Equal(diffDueDate, updatedTodo.DueDate);
	}
	
	[Fact]
	public async Task UpdateTodo_DoesNotUpdateCompletedTodo()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);

		var dueDate = DateTime.Now;
		var todo = new Todo(1, "Test Todo", dueDate, true);
		await _appDbContext.AddAsync(todo);
		await _appDbContext.SaveChangesAsync();

		var diffDueDate = dueDate.AddDays(2);
		var model = new UpdateTodoModel(1, "Updated Todo", diffDueDate);

		// Act
		var exception = await Assert.ThrowsAsync<ArgumentException>(() => todosService.UpdateTodo(model));

		// Assert
		Assert.Equal("Todo is completed", exception.Message);
	}

	[Fact]
	public async Task UpdateTodo_ThrowsWhenTodoNotFound()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);

		var model = new UpdateTodoModel(1, "Updated Todo", DateTime.UtcNow);

		// Act
		var exception = await Assert.ThrowsAsync<ArgumentException>(() => todosService.UpdateTodo(model));

		// Assert
		Assert.StartsWith("Todo not found", exception.Message);
	}

	[Fact]
	public async Task MarkAsDone_MarksTodoAsCompletedInDatabase()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);

		var todo = new Todo(1, "Test Todo", DateTime.Now.AddDays(1), false);
		await _appDbContext.AddAsync(todo);
		await _appDbContext.SaveChangesAsync();

		// Act
		await todosService.MarkAsDone(1);

		// Assert
		Assert.Equal(1, _appDbContext.Todos.Count());
		Assert.True(_appDbContext.Todos.First().IsCompleted);
	}

	[Fact]
	public async Task MarkAsDone_ThrowsWhenTodoNotFound()
	{
		// Arrange
		var todosService = new TodosService(_appDbContext);

		// Act
		var exception = await Assert.ThrowsAsync<ArgumentException>(() => todosService.MarkAsDone(1));

		// Assert
		Assert.StartsWith("Todo not found", exception.Message);
	}
}
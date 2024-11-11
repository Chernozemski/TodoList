using Microsoft.EntityFrameworkCore;
using TodoList.Server.Todos.Entities;

namespace TodoList.Server.Base.Models;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Todo> Todos { get; set; }
}
using Microsoft.EntityFrameworkCore;

namespace TodoList.Server.Base.Models;

public static class MigrationRunner
{
	public static async Task Run(IServiceProvider provider)
	{
		using var scope = provider.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		if ((await context.Database.GetPendingMigrationsAsync()).Any())
			await context.Database.MigrateAsync();
	}
}
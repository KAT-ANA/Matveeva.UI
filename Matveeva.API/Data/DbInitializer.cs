using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;


namespace Matveeva.API.Data
{
	public static class DbInitializer
	{
		public static async Task SeedData(WebApplication app)
		{

			// Uri проекта
			var uri = "https://localhost:7002/";
			// Получение контекста БД
			using var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            ////Выполнение миграций
            //await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.Tovary.Any())
			{
				var _categories = new Category[]
			{
            new Category {GroupName="Верх",
            NormalizedName="Верх"},
            new Category {GroupName="Низ",
            NormalizedName="Низ"},
            new Category {GroupName="Другое",
            NormalizedName="Другое"}
            };

				await context.Categories.AddRangeAsync(_categories);
				await context.SaveChangesAsync();


				var _tovar = new List<Tovar>
		{
            new Tovar { Name = "Куртка",
                Description = "Куртка радуга",
                Image = uri + "Images/1.jpg",
                Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Верх")) },

            new Tovar { Name = "Кофта",
                Description = "Кофта беж",
                Image = uri + "Images/3.jpg",
                Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Верх")) },


            new Tovar {Name = "Кроссовки",
                Description = "Кроссовки син",
                Image = uri + "Images/2.png",
                Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Низ")) },


            new Tovar { Name = "Брюки",
                Description = "Брюки черн",
                Image = uri + "Images/5.jpg",
                Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Низ"))},


            new Tovar {Name = "Сумка",
                Description = "Сумка сердце",
                Image = uri + "Images/9.jpg",
                Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Другое"))}

            };

				await context.Tovary.AddRangeAsync(_tovar);
				await context.SaveChangesAsync();

			}
		}
	}
}

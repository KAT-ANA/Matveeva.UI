using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Entities;
using Shop.Domain.Models;



namespace Matveeva.UI.Services
{
    public class MemoryProductService : IProductService
    {
        List<Tovar> _shop;
        List<Category> _categories;
        IConfiguration _config;



        public MemoryProductService(ICategoryService categoryService, [FromServices] IConfiguration config)
        {
            _config = config;
            _categories = categoryService.GetCategoryListAsync()
                .Result
                .Data;

            SetupData();
        }





        /// <summary>
        /// Инициализация списков
        /// </summary>
        public void SetupData()
        {

            _shop = new List<Tovar>
        {
            new Tovar { Id = 1, Name = "Куртка",
                Description = "Куртка радуга",
                Image = "Images/1.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Верх")).Id },

            new Tovar { Id = 2, Name = "Кофта",
                Description = "Кофта беж",
                Image = "Images/3.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Верх")).Id },


            new Tovar { Id = 3, Name = "Кроссовки",
                Description = "Кроссовки син",
                Image = "Images/2.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Низ")).Id },


            new Tovar { Id = 4, Name = "Брюки",
                Description = "Брюки черн",
                Image = "Images/5.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Низ")).Id },


            new Tovar { Id = 5, Name = "Сумка",
                Description = "Сумка сердце",
                Image = "Images/9.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Другое")).Id }


        };
        }


        Task<ResponseData<ListModel<Tovar>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {


            // Создать объект результата
            var result = new ResponseData<ListModel<Tovar>>();

            // Id категории для фильрации
            int? categoryId = null;

            // если требуется фильтрация, то найти Id категории
            // с заданным categoryNormalizedName
            if (categoryNormalizedName != null)
                categoryId = _categories
                .Find(c =>
                c.NormalizedName.Equals(categoryNormalizedName))
                ?.Id;

            // Выбрать объекты, отфильтрованные по Id категории,
            // если этот Id имеется
            var data = _shop
            .Where(d => categoryId == null || d.CategoryId.Equals(categoryId))?
            .ToList();

            // получить размер страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();


            // получить общее количество страниц
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

            // получить данные страницы
            var listData = new ListModel<Tovar>()
            {
                Items = data.Skip((pageNo - 1) *
            pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // поместить данные в объект результата
            result.Data = listData;



            // Если список пустой
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбраннной категории";
            }
            // Вернуть результат
            return Task.FromResult(result);

        }

        public Task<ResponseData<Tovar>> CreateProductAsync(Tovar product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Tovar>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task UpdateProductAsync(int id, Tovar product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}

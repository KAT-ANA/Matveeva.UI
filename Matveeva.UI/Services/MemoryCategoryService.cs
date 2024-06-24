using Shop.Domain.Entities;
using Shop.Domain.Models;


namespace Matveeva.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
        {
        new Category {Id=1, GroupName="Верх",
        NormalizedName="Верх"},
        new Category {Id=2, GroupName="Низ",
        NormalizedName="Низ"},
        new Category {Id=3, GroupName="Другое",
        NormalizedName="Другое"}

        };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;
using Matveeva.API.Data;

using Shop.Domain.Models;

namespace Matveeva.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TovarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TovarController(AppDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;

        }


        // GET: api/Tovar
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Tovar>>>> GetTovary(
              string? category,
              int pageNo = 1,
              int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Tovar>>();

            // Фильтрация по категории загрузка данных категории
            var data = _context.Tovary
            .Include(d => d.Category)
            .Where(d => String.IsNullOrEmpty(category)
            || d.Category.NormalizedName.Equals(category));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
            if (pageNo > totalPages)
                pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<Tovar>()
            {
                Items = await data
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // поместить данные в объект результата
            result.Data = listData;
            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }
        // GET: api/Tovary/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tovar>> GetTovary(int id)
        {
            var tovar = await _context.Tovary.FindAsync(id);

            if (tovar == null)
            {
                return NotFound();
            }

            return tovar;
        }

        // PUT: api/Tovar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> GetTovary(int id, Tovar tovar)
        {
            if (id != tovar.Id)
            {
                return BadRequest();
            }

            _context.Entry(tovar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TovarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tovar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tovar>> PostTovary(Tovar tovar)
        {
            _context.Tovary.Add(tovar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTovar", new { id = tovar.Id }, tovar);
        }

        // DELETE: api/Tovar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTovary(int id)
        {
            var tovar = await _context.Tovary.FindAsync(id);
            if (tovar == null)
            {
                return NotFound();
            }

            _context.Tovary.Remove(tovar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TovarExists(int id)
        {
            return _context.Tovary.Any(e => e.Id == id);
        }

        [HttpPost("{id}")]

        public async Task<IActionResult> SaveImage(int id, IFormFile image, [FromServices] IWebHostEnvironment env)
        {
            // Найти объект по Id
            var tovar = await _context.Tovary.FindAsync(id);
            if (tovar == null)
            {
                return NotFound();
            }

            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(env.WebRootPath, "Images");

            // получить случайное имя файла
            var randomName = Path.GetRandomFileName();

            // получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);

            // задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);

            // полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);

            // создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);

            // скопировать файл в поток
            await image.CopyToAsync(stream);

            // получить Url хоста
            var host = "https://" + Request.Host;

            // Url файла изображения
            var url = $"{host}/Images/{fileName}";

            // Сохранить url файла в объекте
            tovar.Image = url;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matveeva.UI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Domain.Entities;
using Matveeva.UI.Services;

namespace Matveeva.UI.Areas.Admin.Pages
{
    public class CreateModel(ICategoryService categoryService, IProductService productService) : PageModel
    {
     
            public async Task<IActionResult> OnGet()
            {
                var categoryListData = await categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
                "GroupName");
                return Page();
            }
            [BindProperty]
            public Tovar tovar { get; set; } = default!;
            [BindProperty]
            public IFormFile? Image { get; set; }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                await productService.CreateProductAsync(tovar, Image);
                return RedirectToPage("./Index");
            }
        }
    }

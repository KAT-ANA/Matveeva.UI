using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Matveeva.UI.Data;

using Shop.Domain.Entities;

namespace Matveeva.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Tovar tovar { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tovary == null)
            {
                return NotFound();
            }

            var tovar = await _context.Tovary.FirstOrDefaultAsync(m => m.Id == id);
            if (tovar == null)
            {
                return NotFound();
            }
            else
            {
                tovar = tovar;
            }
            return Page();
        }
    }
}
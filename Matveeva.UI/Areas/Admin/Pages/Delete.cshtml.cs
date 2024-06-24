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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tovar tovar { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tovary == null)
            {
                return NotFound();
            }

            var dish = await _context.Tovary.FirstOrDefaultAsync(m => m.Id == id);

            if (dish == null)
            {
                return NotFound();
            }
            else
            {
                tovar = tovar;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Tovary == null)
            {
                return NotFound();
            }
            var dish = await _context.Tovary.FindAsync(id);

            if (dish != null)
            {
                tovar = tovar;
                _context.Tovary.Remove(tovar);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilms.Models;

namespace TopFilms.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly MovieContext _context;
        public DeleteModel(MovieContext context)
        {
            _context = context;
        }
        public Movie Movie { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                Movie = movie;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }
            var student = await _context.Movies.FindAsync(id);

            if (student != null)
            {
                Movie = student;
                _context.Movies.Remove(Movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

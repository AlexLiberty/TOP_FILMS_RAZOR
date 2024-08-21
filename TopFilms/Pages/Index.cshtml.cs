using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilms.Models;

namespace TopFilms.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MovieContext _context;

        public IndexModel(MovieContext context)
        {
            _context = context;
        }
        public IList<Movie> Movie { get; set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.Movies != null)
            {
                Movie = await _context.Movies.ToListAsync();
            }
        }
    }
}

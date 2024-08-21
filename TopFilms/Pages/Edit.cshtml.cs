using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilms.Models;

namespace TopFilms.Pages
{
    public class EditModel : PageModel
    {
        private readonly MovieContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public EditModel(MovieContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [BindProperty]
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
            Movie = movie;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? imageFile)
        {
            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Movie.Id);

                if (existingMovie == null)
                {
                    return NotFound();
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "Files");
                    var uniqueFileName = $"{Guid.NewGuid().ToString()}_{imageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    Movie.ImagePath = $"/Files/{uniqueFileName}";
                }
                else
                {
                    Movie.ImagePath = existingMovie.ImagePath;
                }

                _context.Update(Movie);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}

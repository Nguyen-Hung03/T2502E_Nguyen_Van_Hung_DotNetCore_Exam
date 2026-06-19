using ComicSys.Data;
using ComicSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicSys.Controllers;

public class ReportsController : Controller
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(new ReportViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ReportViewModel model)
    {
        if (model.EndDate < model.StartDate)
        {
            ModelState.AddModelError(nameof(model.EndDate), "End date must be on or after start date.");
        }

        if (ModelState.IsValid)
        {
            var items = await _context.RentalDetails
                .Include(d => d.Rental)
                    .ThenInclude(r => r!.Customer)
                .Include(d => d.ComicBook)
                .Where(d => d.Rental!.RentalDate.Date >= model.StartDate.Date
                         && d.Rental.RentalDate.Date <= model.EndDate.Date)
                .OrderBy(d => d.Rental!.RentalDate)
                .ThenBy(d => d.ComicBook!.Title)
                .Select(d => new ReportItemViewModel
                {
                    BookName = d.ComicBook!.Title,
                    RentalDate = d.Rental!.RentalDate,
                    ReturnDate = d.Rental.ReturnDate,
                    CustomerName = d.Rental.Customer!.FullName,
                    Quantity = d.Quantity
                })
                .ToListAsync();

            for (var i = 0; i < items.Count; i++)
            {
                items[i].No = i + 1;
            }

            model.Items = items;
        }

        return View(model);
    }
}

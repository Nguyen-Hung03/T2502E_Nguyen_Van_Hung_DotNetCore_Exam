using ComicSys.Data;
using ComicSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComicSys.Controllers;

public class RentalsController : Controller
{
    private readonly AppDbContext _context;

    public RentalsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var rentals = await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.RentalDetails)
                .ThenInclude(d => d.ComicBook)
            .OrderByDescending(r => r.RentalDate)
            .ToListAsync();

        return View(rentals);
    }

    public async Task<IActionResult> Create()
    {
        return View(await BuildCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RentalCreateViewModel model)
    {
        if (model.ReturnDate < model.RentalDate)
        {
            ModelState.AddModelError(nameof(model.ReturnDate), "Return date must be on or after rental date.");
        }

        if (ModelState.IsValid)
        {
            var rental = new Rental
            {
                CustomerID = model.CustomerID,
                RentalDate = model.RentalDate,
                ReturnDate = model.ReturnDate,
                Status = "Đang thuê"
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            var detail = new RentalDetail
            {
                RentalID = rental.RentalID,
                ComicBookID = model.ComicBookID,
                Quantity = model.Quantity,
                PricePerDay = model.PricePerDay
            };

            _context.RentalDetails.Add(detail);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rental created successfully.";
            return RedirectToAction(nameof(Index));
        }

        model.Customers = await GetCustomerSelectList();
        model.ComicBooks = await GetComicBookSelectList();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetPricePerDay(int comicBookId)
    {
        var comicBook = await _context.ComicBooks.FindAsync(comicBookId);
        if (comicBook == null)
        {
            return NotFound();
        }

        return Json(new { pricePerDay = comicBook.PricePerDay });
    }

    private async Task<RentalCreateViewModel> BuildCreateViewModel()
    {
        return new RentalCreateViewModel
        {
            RentalDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(1),
            Quantity = 1,
            Customers = await GetCustomerSelectList(),
            ComicBooks = await GetComicBookSelectList()
        };
    }

    private async Task<IEnumerable<SelectListItem>> GetCustomerSelectList()
    {
        return await _context.Customers
            .OrderBy(c => c.FullName)
            .Select(c => new SelectListItem
            {
                Value = c.CustomerID.ToString(),
                Text = c.FullName
            })
            .ToListAsync();
    }

    private async Task<IEnumerable<SelectListItem>> GetComicBookSelectList()
    {
        return await _context.ComicBooks
            .OrderBy(c => c.Title)
            .Select(c => new SelectListItem
            {
                Value = c.ComicBookID.ToString(),
                Text = c.Title
            })
            .ToListAsync();
    }
}

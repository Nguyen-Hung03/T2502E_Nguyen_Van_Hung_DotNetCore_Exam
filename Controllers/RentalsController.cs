using ComicSys.Data;
using ComicSys.Models;
using Microsoft.AspNetCore.Mvc;
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
            ModelState.AddModelError(nameof(model.ReturnDate), "Ngày trả phải sau hoặc bằng ngày thuê.");
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.FullName.ToLower() == model.CustomerName.Trim().ToLower());

        if (customer == null)
        {
            ModelState.AddModelError(nameof(model.CustomerName), "Không tìm thấy khách hàng. Vui lòng kiểm tra lại tên.");
        }

        var comicBook = await _context.ComicBooks
            .FirstOrDefaultAsync(c => c.Title.ToLower() == model.ComicBookTitle.Trim().ToLower());

        if (comicBook == null)
        {
            ModelState.AddModelError(nameof(model.ComicBookTitle), "Không tìm thấy truyện. Vui lòng kiểm tra lại tên.");
        }

        if (ModelState.IsValid && customer != null && comicBook != null)
        {
            var pricePerDay = model.PricePerDay > 0 ? model.PricePerDay : comicBook.PricePerDay;

            var rental = new Rental
            {
                CustomerID = customer.CustomerID,
                RentalDate = model.RentalDate,
                ReturnDate = model.ReturnDate,
                Status = "Đang thuê"
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            _context.RentalDetails.Add(new RentalDetail
            {
                RentalID = rental.RentalID,
                ComicBookID = comicBook.ComicBookID,
                Quantity = model.Quantity,
                PricePerDay = pricePerDay
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tạo phiếu thuê thành công.";
            return RedirectToAction(nameof(Index));
        }

        await LoadSuggestions(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetPricePerDay(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest();
        }

        var comicBook = await _context.ComicBooks
            .FirstOrDefaultAsync(c => c.Title.ToLower() == title.Trim().ToLower());

        if (comicBook == null)
        {
            return NotFound();
        }

        return Json(new { pricePerDay = comicBook.PricePerDay });
    }

    private async Task<RentalCreateViewModel> BuildCreateViewModel()
    {
        var model = new RentalCreateViewModel
        {
            RentalDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(1),
            Quantity = 1
        };

        await LoadSuggestions(model);
        return model;
    }

    private async Task LoadSuggestions(RentalCreateViewModel model)
    {
        model.CustomerSuggestions = await _context.Customers
            .OrderBy(c => c.FullName)
            .Select(c => c.FullName)
            .ToListAsync();

        model.ComicBookSuggestions = await _context.ComicBooks
            .OrderBy(c => c.Title)
            .Select(c => c.Title)
            .ToListAsync();
    }
}

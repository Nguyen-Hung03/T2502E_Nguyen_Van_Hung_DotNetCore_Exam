using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComicSys.Models;

public class RentalCreateViewModel
{
    [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
    [Display(Name = "Khách hàng")]
    public int CustomerID { get; set; }

    [Required]
    [Display(Name = "Ngày thuê")]
    [DataType(DataType.Date)]
    public DateTime RentalDate { get; set; } = DateTime.Today;

    [Required]
    [Display(Name = "Ngày trả")]
    [DataType(DataType.Date)]
    public DateTime ReturnDate { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "Vui lòng chọn truyện")]
    [Display(Name = "Truyện tranh")]
    public int ComicBookID { get; set; }

    [Required]
    [Display(Name = "Số lượng")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ít nhất là 1")]
    public int Quantity { get; set; } = 1;

    [Required]
    [Display(Name = "Giá thuê / ngày")]
    public decimal PricePerDay { get; set; }

    public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> ComicBooks { get; set; } = new List<SelectListItem>();
}

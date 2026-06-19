using System.ComponentModel.DataAnnotations;

namespace ComicSys.Models;

public class RentalCreateViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
    [Display(Name = "Tên khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập tên truyện")]
    [Display(Name = "Tên truyện")]
    public string ComicBookTitle { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Ngày thuê")]
    [DataType(DataType.Date)]
    public DateTime RentalDate { get; set; } = DateTime.Today;

    [Required]
    [Display(Name = "Ngày trả")]
    [DataType(DataType.Date)]
    public DateTime ReturnDate { get; set; } = DateTime.Today.AddDays(1);

    [Required]
    [Display(Name = "Số lượng")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ít nhất là 1")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Giá thuê / ngày")]
    public decimal PricePerDay { get; set; }

    public List<string> CustomerSuggestions { get; set; } = new();
    public List<string> ComicBookSuggestions { get; set; } = new();
}

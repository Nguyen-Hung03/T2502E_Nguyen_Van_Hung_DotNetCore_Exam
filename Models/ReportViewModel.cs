using System.ComponentModel.DataAnnotations;

namespace ComicSys.Models;

public class ReportViewModel
{
    [Required]
    [Display(Name = "Từ ngày")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-1);

    [Required]
    [Display(Name = "Đến ngày")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today;

    public List<ReportItemViewModel> Items { get; set; } = new();
}

public class ReportItemViewModel
{
    public int No { get; set; }
    public string BookName { get; set; } = string.Empty;
    public DateTime RentalDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicSys.Models;

[Table("Customers")]
public class Customer
{
    [Key]
    public int CustomerID { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(255)]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [StringLength(15)]
    [Display(Name = "Số điện thoại")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Ngày đăng ký")]
    [DataType(DataType.Date)]
    public DateTime RegistrationDate { get; set; } = DateTime.Today;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicSys.Models;

[Table("Rentals")]
public class Rental
{
    [Key]
    public int RentalID { get; set; }

    [Required]
    [Display(Name = "Customer")]
    public int CustomerID { get; set; }

    [Required]
    [Display(Name = "Rental date")]
    [DataType(DataType.Date)]
    public DateTime RentalDate { get; set; } = DateTime.Today;

    [Required]
    [Display(Name = "Return date")]
    [DataType(DataType.Date)]
    public DateTime ReturnDate { get; set; } = DateTime.Today.AddDays(1);

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Đang thuê";

    [ForeignKey(nameof(CustomerID))]
    public Customer? Customer { get; set; }

    public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
}

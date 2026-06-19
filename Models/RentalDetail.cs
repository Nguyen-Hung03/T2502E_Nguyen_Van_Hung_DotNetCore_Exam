using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicSys.Models;

[Table("RentalDetails")]
public class RentalDetail
{
    [Key]
    public int RentalDetailID { get; set; }

    [Required]
    public int RentalID { get; set; }

    [Required]
    [Display(Name = "Comic book")]
    public int ComicBookID { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    [Display(Name = "Price per day")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerDay { get; set; }

    [ForeignKey(nameof(RentalID))]
    public Rental? Rental { get; set; }

    [ForeignKey(nameof(ComicBookID))]
    public ComicBook? ComicBook { get; set; }
}

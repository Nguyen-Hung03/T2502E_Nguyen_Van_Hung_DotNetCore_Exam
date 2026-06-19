using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicSys.Models;

[Table("ComicBooks")]
public class ComicBook
{
    [Key]
    public int ComicBookID { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên truyện")]
    [StringLength(255)]
    [Display(Name = "Tên truyện")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập tác giả")]
    [StringLength(255)]
    [Display(Name = "Tác giả")]
    public string Author { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Giá thuê / ngày")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal PricePerDay { get; set; }

    public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
}

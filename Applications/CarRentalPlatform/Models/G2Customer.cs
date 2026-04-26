using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.Models;

public partial class G2Customer
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter a first name")]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a last name")]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
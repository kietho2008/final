using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2CarRentalCustomers.Models;

[Table("Customer")]
public partial class G2Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is mandatory")]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last Name is mandatory")]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
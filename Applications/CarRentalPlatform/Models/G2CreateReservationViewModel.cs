using System.ComponentModel.DataAnnotations;

namespace CarRentalPlatform.Models
{
	public class G2CreateReservationViewModel
	{
		[Required]
		[Display(Name = "Customer")]
		public int CustomerId { get; set; }

		[Required]
		[Display(Name = "Vehicle")]
		public int VehicleId { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[Display(Name = "End Date")]
		public DateTime EndDate { get; set; }
	}
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRentalPlatform.Models
{
	public class G2ReservationsPageViewModel
	{
		public G2CreateReservationViewModel Reservation { get; set; } = new G2CreateReservationViewModel
		{
			StartDate = DateTime.Today,
			EndDate = DateTime.Today.AddDays(1)
		};

		public List<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
		public List<SelectListItem> Vehicles { get; set; } = new List<SelectListItem>();
		public List<G2ReservationHistoryViewModel> ReservationHistory { get; set; } = new List<G2ReservationHistoryViewModel>();

		public int? SelectedCustomerId { get; set; }
	}
}

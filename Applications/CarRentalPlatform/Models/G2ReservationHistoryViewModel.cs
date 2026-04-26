namespace CarRentalPlatform.Models
{
	public class G2ReservationHistoryViewModel
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		public int VehicleId { get; set; }
		public string VehicleCode { get; set; } = string.Empty;
		public string VehicleType { get; set; } = string.Empty;
		public string VehicleStatus { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}

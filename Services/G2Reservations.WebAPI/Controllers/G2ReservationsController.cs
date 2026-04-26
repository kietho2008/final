using G2Reservations.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace G2Reservations.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class G2ReservationsController : ControllerBase
	{
		private readonly G2ReservationDbContext _context;
		private readonly IHttpClientFactory _httpClientFactory;

		public G2ReservationsController(G2ReservationDbContext context, IHttpClientFactory httpClientFactory)
		{
			_context = context;
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<G2ReservationDto>>> GetReservations()
		{
			var reservations = await _context.Reservations
				.OrderByDescending(r => r.CreatedDate)
				.Select(r => new G2ReservationDto
				{
					Id = r.Id,
					CustomerId = r.CustomerId,
					VehicleId = r.VehicleId,
					CreatedDate = r.CreatedDate,
					StartDate = r.StartDate,
					EndDate = r.EndDate
				})
				.ToListAsync();

			return Ok(reservations);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<G2ReservationDto>> GetReservation(int id)
		{
			var reservation = await _context.Reservations.FindAsync(id);

			if (reservation == null)
			{
				return NotFound(new
				{
					error = "NotFound",
					message = "Reservation not found."
				});
			}

			return Ok(new G2ReservationDto
			{
				Id = reservation.Id,
				CustomerId = reservation.CustomerId,
				VehicleId = reservation.VehicleId,
				CreatedDate = reservation.CreatedDate,
				StartDate = reservation.StartDate,
				EndDate = reservation.EndDate
			});
		}

		[HttpGet("customer/{customerId}")]
		public async Task<ActionResult<IEnumerable<G2ReservationDto>>> GetReservationsByCustomer(int customerId)
		{
			var reservations = await _context.Reservations
				.Where(r => r.CustomerId == customerId)
				.OrderByDescending(r => r.CreatedDate)
				.Select(r => new G2ReservationDto
				{
					Id = r.Id,
					CustomerId = r.CustomerId,
					VehicleId = r.VehicleId,
					CreatedDate = r.CreatedDate,
					StartDate = r.StartDate,
					EndDate = r.EndDate
				})
				.ToListAsync();

			return Ok(reservations);
		}

		[HttpGet("customer/{customerId}/history")]
		public async Task<ActionResult<IEnumerable<G2ReservationHistoryDto>>> GetReservationHistoryByCustomer(int customerId)
		{
			var customerClient = _httpClientFactory.CreateClient("CustomersApi");
			var vehicleClient = _httpClientFactory.CreateClient("VehicleInventoryApi");

			var customerResponse = await customerClient.GetAsync($"api/G2CustomersApi/{customerId}");
			if (!customerResponse.IsSuccessStatusCode)
			{
				return NotFound(new
				{
					error = "NotFound",
					message = "Customer not found."
				});
			}

			var customer = await customerResponse.Content.ReadFromJsonAsync<G2CustomerLookupDto>();
			if (customer == null)
			{
				return NotFound(new
				{
					error = "NotFound",
					message = "Customer not found."
				});
			}

			var reservations = await _context.Reservations
				.Where(r => r.CustomerId == customerId)
				.OrderByDescending(r => r.CreatedDate)
				.ToListAsync();

			var result = new List<G2ReservationHistoryDto>();

			foreach (var reservation in reservations)
			{
				var vehicleResponse = await vehicleClient.GetAsync($"api/G2Vehicles/{reservation.VehicleId}");
				G2VehicleLookupDto? vehicle = null;

				if (vehicleResponse.IsSuccessStatusCode)
				{
					vehicle = await vehicleResponse.Content.ReadFromJsonAsync<G2VehicleLookupDto>();
				}

				result.Add(new G2ReservationHistoryDto
				{
					Id = reservation.Id,
					CustomerId = reservation.CustomerId,
					CustomerName = $"{customer.FirstName} {customer.LastName}".Trim(),
					VehicleId = reservation.VehicleId,
					VehicleCode = vehicle?.VehicleCode ?? "Unknown",
					VehicleType = vehicle?.VehicleType ?? "Unknown",
					VehicleStatus = vehicle?.VehicleStatus ?? "Unknown",
					CreatedDate = reservation.CreatedDate,
					StartDate = reservation.StartDate,
					EndDate = reservation.EndDate
				});
			}

			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<G2ReservationDto>> PostReservation([FromBody] G2CreateReservationDto dto)
		{
			if (dto.CustomerId <= 0)
			{
				return BadRequest(new
				{
					error = "InvalidParameter",
					message = "CustomerId must be greater than zero."
				});
			}

			if (dto.VehicleId <= 0)
			{
				return BadRequest(new
				{
					error = "InvalidParameter",
					message = "VehicleId must be greater than zero."
				});
			}

			if (dto.StartDate >= dto.EndDate)
			{
				return BadRequest(new
				{
					error = "InvalidParameter",
					message = "End date must be greater than start date."
				});
			}

			var customerClient = _httpClientFactory.CreateClient("CustomersApi");
			var vehicleClient = _httpClientFactory.CreateClient("VehicleInventoryApi");

			var customerResponse = await customerClient.GetAsync($"api/G2CustomersApi/{dto.CustomerId}");
			if (!customerResponse.IsSuccessStatusCode)
			{
				return NotFound(new
				{
					error = "NotFound",
					message = "Customer not found."
				});
			}

			var vehicleResponse = await vehicleClient.GetAsync($"api/G2Vehicles/{dto.VehicleId}");
			if (!vehicleResponse.IsSuccessStatusCode)
			{
				return NotFound(new
				{
					error = "NotFound",
					message = "Vehicle not found."
				});
			}

			var vehicle = await vehicleResponse.Content.ReadFromJsonAsync<G2VehicleLookupDto>();
			if (vehicle == null)
			{
				return NotFound(new
				{
					error = "NotFound",
					message = "Vehicle not found."
				});
			}

			if (string.Equals(vehicle.VehicleStatus, "Reserved", StringComparison.OrdinalIgnoreCase))
			{
				return BadRequest(new
				{
					error = "InvalidReservation",
					message = "Vehicle is already reserved."
				});
			}

			if (string.Equals(vehicle.VehicleStatus, "Maintenance", StringComparison.OrdinalIgnoreCase))
			{
				return BadRequest(new
				{
					error = "InvalidReservation",
					message = "Vehicle is currently being serviced."
				});
			}

			var overlappingReservation = await _context.Reservations.AnyAsync(r =>
				r.VehicleId == dto.VehicleId &&
				dto.StartDate < r.EndDate &&
				dto.EndDate > r.StartDate);

			if (overlappingReservation)
			{
				return BadRequest(new
				{
					error = "InvalidReservation",
					message = "Vehicle is already reserved for the selected dates."
				});
			}

			var reservation = new G2Reservation
			{
				CustomerId = dto.CustomerId,
				VehicleId = dto.VehicleId,
				CreatedDate = DateTime.UtcNow,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate
			};

			_context.Reservations.Add(reservation);
			await _context.SaveChangesAsync();

			var updateStatusResponse = await vehicleClient.PutAsJsonAsync(
				$"api/G2Vehicles/{dto.VehicleId}/status",
				"Reserved");

			if (!updateStatusResponse.IsSuccessStatusCode)
			{
				_context.Reservations.Remove(reservation);
				await _context.SaveChangesAsync();

				var updateError = await updateStatusResponse.Content.ReadAsStringAsync();

				return StatusCode(500, new
				{
					error = "VehicleStatusUpdateFailed",
					message = string.IsNullOrWhiteSpace(updateError)
						? "Reservation could not be completed because the vehicle status update failed."
						: updateError
				});
			}

			var result = new G2ReservationDto
			{
				Id = reservation.Id,
				CustomerId = reservation.CustomerId,
				VehicleId = reservation.VehicleId,
				CreatedDate = reservation.CreatedDate,
				StartDate = reservation.StartDate,
				EndDate = reservation.EndDate
			};

			return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, result);
		}
	}
}
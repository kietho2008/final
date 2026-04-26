using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using G2VehicleInventory.Application.DTOs;
using G2VehicleInventory.Application.Services;
using G2VehicleInventory.Domain.Enums;

namespace G2VehicleInventory.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class G2VehiclesController : ControllerBase
	{
		private readonly G2GetAllVehicles _getAllVehiclesService;
		private readonly G2GetVehicleById _getVehicleByIdService;
		private readonly G2UpdateVehicleStatus _updateVehicleStatusService;
		private readonly G2CreateVehicle _CreateVehicleService;
		private readonly G2DeleteVehicle _DeleteVehicleService;

		public G2VehiclesController(

			G2GetAllVehicles getAllVehiclesService,
			G2GetVehicleById getVehicleByIdService,
			G2UpdateVehicleStatus updateVehicleStatusService,
			G2CreateVehicle createVehicleService,
			G2DeleteVehicle deleteVehicleService)
		{
			_getAllVehiclesService = getAllVehiclesService;
			_getVehicleByIdService = getVehicleByIdService;
			_updateVehicleStatusService = updateVehicleStatusService;
			_CreateVehicleService = createVehicleService;
			_DeleteVehicleService = deleteVehicleService;
		}

		[HttpPost]
		public async Task<ActionResult<G2VehicleDto>> CreateVehicle([FromBody] G2CreateVehicleDto dto)
		{
			await _CreateVehicleService.CreateVehicle(dto);
			return Ok(dto);
		}

		[HttpGet]
		public async Task<ActionResult<List<G2VehicleDto>>> GetAllVehicles()
		{
			var vehicles = await _getAllVehiclesService.GetAllVehicles();
			return Ok(vehicles);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<G2VehicleDto>> GetVehicleById(int id)
		{
			var vehicle = await _getVehicleByIdService.GetVehicleById(id);
			return Ok(vehicle);
		}

		[HttpPut("{id}/status")]
		public async Task<ActionResult<G2VehicleDto>> UpdateVehicleStatus(int id, [FromBody] VehicleStatus newStatus)
		{
			var updatedVehicle = await _updateVehicleStatusService.UpdateVehicleStatus(id, newStatus);
			return Ok(updatedVehicle);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteVehicle(int id)
		{
			await _DeleteVehicleService.DeleteVehicle(id);
			return NoContent();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Application.DTOs;
using G2VehicleInventory.Application.Interfaces;

namespace G2VehicleInventory.Application.Services
{
	public class G2GetAllVehicles
	{
		private readonly G2IVehicleRepository _vehicleRepository;

		public G2GetAllVehicles(G2IVehicleRepository vehicleRepository)
		{
			_vehicleRepository = vehicleRepository;
		}

		public async Task<List<G2VehicleDto>> GetAllVehicles()
		{
			var vehicles = await _vehicleRepository.GetAll();
			return vehicles.Select(vehicle => new G2VehicleDto
			{
				Id = vehicle.Id,
				VehicleCode = vehicle.VehicleCode.Value,
				LocationId = vehicle.LocationId,
				VehicleType = vehicle.VehicleType,
				VehicleStatus = vehicle.VehicleStatus
			}).ToList();
		}

	}
}

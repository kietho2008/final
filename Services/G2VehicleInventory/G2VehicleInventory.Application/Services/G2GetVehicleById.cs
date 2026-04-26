using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Application.DTOs;
using G2VehicleInventory.Application.Exceptions;
using G2VehicleInventory.Application.Interfaces;


namespace G2VehicleInventory.Application.Services
{
	public class G2GetVehicleById
	{
		private readonly G2IVehicleRepository _vehicleRepository;

		public G2GetVehicleById(G2IVehicleRepository vehicleRepository)
		{
			_vehicleRepository = vehicleRepository;
		}

		public async Task<G2VehicleDto> GetVehicleById(int id)
		{
			var vehicle = await _vehicleRepository.GetById(id);

			if (vehicle == null)
			{
				throw new G2NotFoundException($"Vehicle with ID {id} not found.");
			}
			return new G2VehicleDto
			{
				Id = vehicle.Id,
				VehicleCode = vehicle.VehicleCode.Value,
				LocationId = vehicle.LocationId,
				VehicleType = vehicle.VehicleType,
				VehicleStatus = vehicle.VehicleStatus
			};
		}
	}
}

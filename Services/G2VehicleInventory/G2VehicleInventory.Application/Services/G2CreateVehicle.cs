using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Application.DTOs;
using G2VehicleInventory.Application.Interfaces;
using G2VehicleInventory.Domain.Enums;
using G2VehicleInventory.Domain.ValueObjects;
using G2VehicleInventory.Domain.VehicleAggregate;

namespace G2VehicleInventory.Application.Services
{
	public class G2CreateVehicle
	{
		private readonly G2IVehicleRepository _vehicleRepository;

		public G2CreateVehicle(G2IVehicleRepository vehicleRepository)
		{
			_vehicleRepository = vehicleRepository;
		}

		public async Task CreateVehicle(G2CreateVehicleDto dto)
		{
			var vehicleCode = new G2VehicleCode(dto.VehicleCode);

			var vehicle = new G2Vehicle
			(
				vehicleCode,
				dto.LocationId,
				dto.VehicleType,
				VehicleStatus.Available // Should be available when created, don't want the client to set it
			);

			await _vehicleRepository.Add(vehicle);
			await _vehicleRepository.SaveChanges();
		}

	}
}

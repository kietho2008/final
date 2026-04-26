using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Application.DTOs;
using G2VehicleInventory.Application.Exceptions;
using G2VehicleInventory.Application.Interfaces;
using G2VehicleInventory.Domain.Enums;

namespace G2VehicleInventory.Application.Services
{
	public class G2UpdateVehicleStatus
	{
		private readonly G2IVehicleRepository _vehicleRepository;

		public G2UpdateVehicleStatus(G2IVehicleRepository vehicleRepository)
		{
			_vehicleRepository = vehicleRepository;
		}

		public async Task<G2VehicleDto> UpdateVehicleStatus(int id, VehicleStatus newStatus)
		{
			var currentVehicle = await _vehicleRepository.GetById(id);

			if (currentVehicle == null)
			{
				throw new Exception($"Vehicle with id {id} not found.");
			}
			switch (newStatus)
			{
				case VehicleStatus.Available:
					currentVehicle.MarkAvailable();
					break;
				case VehicleStatus.Rented:
					currentVehicle.MarkRented();
					break;
				case VehicleStatus.Reserved:
					currentVehicle.MarkReserved();
					break;
				case VehicleStatus.Maintenance:
					currentVehicle.MarkServiced();
					break;
				default:
					throw new G2InvalidVehicleStatusException($"Invalid vehicle status: {newStatus}");
			}

			await _vehicleRepository.SaveChanges();

			return new G2VehicleDto
			{
				Id = currentVehicle.Id,
				VehicleCode = currentVehicle.VehicleCode.Value,
				LocationId = currentVehicle.LocationId,
				VehicleType = currentVehicle.VehicleType,
				VehicleStatus = currentVehicle.VehicleStatus
			};



		}

	}
}

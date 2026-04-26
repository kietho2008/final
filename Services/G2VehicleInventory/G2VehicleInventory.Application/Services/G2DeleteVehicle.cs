using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Application.Interfaces;

namespace G2VehicleInventory.Application.Services
{
	public class G2DeleteVehicle
	{
		private readonly G2IVehicleRepository _vehicleRepository;

		public G2DeleteVehicle(G2IVehicleRepository vehicleRepository)
		{
			_vehicleRepository = vehicleRepository;
		}

		public async Task DeleteVehicle(int id)
		{
			var vehicle = await _vehicleRepository.GetById(id);
			// GetVehicleById already checks for null so i dont need it again.

			await _vehicleRepository.Delete(id);
			await _vehicleRepository.SaveChanges();
		}


	}
}

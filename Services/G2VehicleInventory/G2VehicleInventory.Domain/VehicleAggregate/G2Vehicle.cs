using G2VehicleInventory.Domain.Enums;
using G2VehicleInventory.Domain.Exceptions;
using G2VehicleInventory.Domain.ValueObjects;

namespace G2VehicleInventory.Domain.VehicleAggregate
{
	public class G2Vehicle
	{
		public int Id { get; private set; }
		public G2VehicleCode VehicleCode { get; private set; }
		public int LocationId { get; private set; }
		public VehicleType VehicleType { get; private set; }
		public VehicleStatus VehicleStatus { get; private set; }

		public G2Vehicle(G2VehicleCode vehicleCode, int locationId, VehicleType vehicleType, VehicleStatus vehicleStatus)
		{
			this.VehicleCode = vehicleCode;
			this.LocationId = locationId;
			this.VehicleType = vehicleType;
			this.VehicleStatus = vehicleStatus;
		}

		//for ef core
		private G2Vehicle() { }

		public void MarkAvailable()
		{
			if (this.VehicleStatus == VehicleStatus.Reserved)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a reserved vehicle as available.");
			}
			else
			{
				this.VehicleStatus = VehicleStatus.Available;
			}

		}

		public void MarkRented()
		{
			if (this.VehicleStatus == VehicleStatus.Rented)
			{
				throw new G2InvalidVehicleStatusChangeException("Vehicle is already rented.");
			}

			if (this.VehicleStatus == VehicleStatus.Reserved)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a reserved vehicle as rented.");
			}

			if (this.VehicleStatus == VehicleStatus.Maintenance)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a vehicle under maintenance as rented.");
			}

			this.VehicleStatus = VehicleStatus.Rented;

		}

		public void MarkReserved()
		{
			if (this.VehicleStatus == VehicleStatus.Rented)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a rented vehicle as reserved.");
			}
			if (this.VehicleStatus == VehicleStatus.Maintenance)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a vehicle under maintenance as reserved.");
			}
			if (this.VehicleStatus == VehicleStatus.Reserved)
			{
				throw new G2InvalidVehicleStatusChangeException("Vehicle is already reserved.");
			}
			this.VehicleStatus = VehicleStatus.Reserved;
		}

		public void MarkServiced()
		{
			if (this.VehicleStatus == VehicleStatus.Rented)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a rented vehicle as under maintenance.");
			}
			if (this.VehicleStatus == VehicleStatus.Reserved)
			{
				throw new G2InvalidVehicleStatusChangeException("Cannot mark a reserved vehicle as under maintenance.");
			}
			this.VehicleStatus = VehicleStatus.Maintenance;
		}
	}
}

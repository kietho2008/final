using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2VehicleInventory.Domain.Exceptions
{
	public class G2InvalidVehicleStatusChangeException : Exception
	{
		public G2InvalidVehicleStatusChangeException(string message) : base(message)
		{
		}
	}
}

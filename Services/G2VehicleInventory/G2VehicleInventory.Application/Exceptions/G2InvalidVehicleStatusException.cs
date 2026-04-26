using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2VehicleInventory.Application.Exceptions
{
	internal class G2InvalidVehicleStatusException : Exception
	{
		public G2InvalidVehicleStatusException(string message) : base(message)
		{
		}
	}
}

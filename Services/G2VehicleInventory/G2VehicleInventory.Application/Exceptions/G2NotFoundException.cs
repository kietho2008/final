using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2VehicleInventory.Application.Exceptions
{
	internal class G2NotFoundException : Exception
	{
		public G2NotFoundException(string message) : base(message)
		{
		}
	}
}

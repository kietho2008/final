using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G2VehicleInventory.Domain.ValueObjects;

namespace G2VehicleInventory.Domain.LocationAggregate
{
	public class G2Location
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public G2Address Address { get; private set; } // Uses the Value Object

		public G2Location(string name, G2Address address)
		{
			Name = name;
			Address = address;
		}

		//for ef core
		private G2Location() { }
	}
}

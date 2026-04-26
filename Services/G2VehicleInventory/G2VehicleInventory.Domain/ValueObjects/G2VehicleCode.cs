using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2VehicleInventory.Domain.ValueObjects
{
	public class G2VehicleCode
	{
		public string Value { get; private set; }

		public G2VehicleCode(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Vehicle Code cannot be empty.");

			if (value.Length > 10)
				throw new ArgumentException("Vehicle Code is too long.");

			Value = value.ToUpper();
		}


		public override bool Equals(object? obj)
		{
			if (obj is G2VehicleCode other)
				return Value == other.Value;
			return false;
		}

		public override int GetHashCode() => Value.GetHashCode();

		public static bool operator ==(G2VehicleCode? left, G2VehicleCode? right)
		{
			if (left is null && right is null) return true;
			if (left is null || right is null) return false;
			return left.Value == right.Value;
		}

		public static bool operator !=(G2VehicleCode? left, G2VehicleCode? right)
		{
			return !(left == right);
		}
	}

}

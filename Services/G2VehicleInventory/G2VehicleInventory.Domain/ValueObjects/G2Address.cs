using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2VehicleInventory.Domain.ValueObjects
{
	public class G2Address
	{
		public string Street { get; private set; }
		public string City { get; private set; }
		public string PostalCode { get; private set; }

		public G2Address(string street, string city, string postalCode)
		{
			if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street is required.");
			if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.");
			if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("Postal Code is required.");

			Street = street;
			City = city;
			PostalCode = postalCode;
		}

		public override bool Equals(object? obj)
		{
			if (obj is G2Address other)
			{
				return Street == other.Street &&
					   City == other.City &&
					   PostalCode == other.PostalCode;
			}
			return false;
		}

		public override int GetHashCode() => HashCode.Combine(Street, City, PostalCode);

		public static bool operator ==(G2Address? left, G2Address? right)
		{
			if (left is null && right is null) return true;
			if (left is null || right is null) return false;
			return left.Equals(right);
		}

		public static bool operator !=(G2Address? left, G2Address? right) => !(left == right);
	}

}

using G2VehicleInventory.Domain.VehicleAggregate;

namespace G2VehicleInventory.Application.Interfaces
{
	public interface G2IVehicleRepository
	{
		Task<G2Vehicle> GetById(int id);
		Task<List<G2Vehicle>> GetAll();
		Task Add(G2Vehicle vehicle);
		Task Delete(int id);
		Task SaveChanges();


	}
}

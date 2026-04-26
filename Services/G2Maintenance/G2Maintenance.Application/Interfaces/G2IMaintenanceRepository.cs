using G2Maintenance.Domain.HistoryAggregate;

namespace G2Maintenance.Application.Interfaces;

public interface G2IMaintenanceRepository
{
    Task<List<G2RepairHistory>> GetById(int id);
    Task Add(G2RepairHistory repairHistory);
}
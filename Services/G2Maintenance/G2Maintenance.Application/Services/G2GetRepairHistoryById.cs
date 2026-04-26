using G2Maintenance.Application.DTOs;
using G2Maintenance.Application.Interfaces;

namespace G2Maintenance.Application.Services;

public class G2GetRepairHistoryById
{
    private readonly G2IMaintenanceRepository _maintenanceRepository;

    public G2GetRepairHistoryById(G2IMaintenanceRepository maintenanceRepository)
    {
        _maintenanceRepository = maintenanceRepository;
    }

    public async Task<List<G2RepairHistoryDTO>> GetRepairHistoryById(int id)
    {
        var history = await _maintenanceRepository.GetById(id);
        return history.Select(h => new G2RepairHistoryDTO
        {
            Id = h.Id,
            VehicleId = h.VehicleId,
            Cost =  h.Cost,
            Description = h.Description,
            PerformedBy = h.PerformedBy,
            RepairDate = h.RepairDate
        }).ToList();
    }
}
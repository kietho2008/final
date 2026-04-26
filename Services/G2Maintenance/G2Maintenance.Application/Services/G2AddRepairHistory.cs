using G2Maintenance.Application.DTOs;
using G2Maintenance.Application.Interfaces;
using G2Maintenance.Domain.HistoryAggregate;

namespace G2Maintenance.Application.Services;

public class G2AddRepairHistory
{
    private readonly G2IMaintenanceRepository _maintenanceRepository;

    public G2AddRepairHistory(G2IMaintenanceRepository maintenanceRepository)
    {
        _maintenanceRepository = maintenanceRepository;
    }

    public async Task AddHistory(G2AddRepairHistoryDTO dto)
    {
        var history = new G2RepairHistory(dto.VehicleId, dto.RepairDate, dto.Description, dto.Cost, dto.PerformedBy);
        await _maintenanceRepository.Add(history);
    }
}
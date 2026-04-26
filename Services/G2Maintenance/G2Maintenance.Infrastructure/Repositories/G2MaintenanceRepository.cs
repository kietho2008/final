using G2Maintenance.Application.Interfaces;
using G2Maintenance.Domain.HistoryAggregate;
using G2Maintenance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace G2Maintenance.Infrastructure.Repositories;

public class G2MaintenanceRepository : G2IMaintenanceRepository
{
    private readonly G2MaintenanceDbContext _context;

    public G2MaintenanceRepository(G2MaintenanceDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<G2RepairHistory>> GetById(int id)
    {
        return await _context.RepairHistories
            .Where(x => x.VehicleId == id).ToListAsync();
    }

    public async Task Add(G2RepairHistory repairHistory)
    {
        await _context.RepairHistories.AddAsync(repairHistory);
        await SaveChanges();
    }
    
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
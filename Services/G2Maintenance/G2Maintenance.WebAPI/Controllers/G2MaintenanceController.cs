using G2Maintenance.Application.DTOs;
using G2Maintenance.Application.Interfaces;
using G2Maintenance.Application.Services;
using G2Maintenance.Domain.HistoryAggregate;
using Microsoft.AspNetCore.Mvc;

namespace G2Maintenance.WebAPI.Controllers;

[Route("api/maintenance")]
[ApiController]
public class G2MaintenanceController : ControllerBase
{
    private readonly G2AddRepairHistory _addService;
    private readonly G2GetRepairHistoryById _getByIdService;
    public G2MaintenanceController(G2AddRepairHistory addService, G2GetRepairHistoryById getByIdService)
    {
        _addService = addService;
        _getByIdService = getByIdService;
    }
    [HttpGet("vehicles/{vehicleId}/repairs")]
    public async Task<ActionResult> GetRepairHistory(int vehicleId)
    {
        var history = await _getByIdService.GetRepairHistoryById(vehicleId);
        return Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> AddRepair([FromBody] G2AddRepairHistoryDTO g2Repair)
    {
        await _addService.AddHistory(g2Repair);
        return Created();
    }
}
namespace G2Maintenance.Domain.HistoryAggregate;

public class G2RepairHistory
{
    public int Id { get; private set; }
    public int VehicleId { get; private set; }
    public DateTime RepairDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Cost { get; private set; }
    public string PerformedBy { get; private set; } = string.Empty;

    public G2RepairHistory(int vehicleId, DateTime repairDate, string description, decimal cost, string performedBy)
    {
        if (vehicleId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(vehicleId));
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentOutOfRangeException(nameof(description));
        }
        if (cost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }
        this.VehicleId = vehicleId;
        this.RepairDate = repairDate;
        this.Description = description;
        this.Cost = cost;
        this.PerformedBy = performedBy;
    }

    private G2RepairHistory()
    {
    }
}
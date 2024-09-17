namespace WebAppKafka.Models;

public partial class Rider
{
    public Guid Id { get; set; }

    public string? RiderName { get; set; }


    public string? Organization { get; set; }

    public virtual ICollection<RiderLocationLog> RiderLocationLogs { get; set; } = new List<RiderLocationLog>();


}

public partial class RiderWithGroup : Rider
{
    public string? GroupId { get; set; }
    public RiderLocationLog getRiderLocationLogs()
    {
        var random = new Random();
        return new RiderLocationLog
        {
            Id = Guid.NewGuid(),
            GroupId = GroupId,
            RiderId = this.Id,
            Coordinates = $"Latitude = , {random.NextDouble() * 180 - 90}, Longitude = {random.NextDouble() * 360 - 180}"
        };
    }
}

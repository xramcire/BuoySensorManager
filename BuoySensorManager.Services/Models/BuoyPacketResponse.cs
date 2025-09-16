namespace BuoySensorManager.Services.Models
{
    public record BuoyPacketResponse
    {
        public Guid Id { get; init; } = default!;

        public int Port { get; init; } = default!;

        public string BuoyName { get; init; } = default!;

        public double Depth { get; init; }

        public DateTime ReadingOn { get; init; }
    }
}

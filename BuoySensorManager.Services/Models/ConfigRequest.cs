using BuoySensorManager.Services.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BuoySensorManager.Services.Models
{
    public record ConfigRequest
    {
        [Required, IpAddress]
        public string BuoySensorEcbAddress { get; set; } = default!;

        [Required, Range(1, 65535)]
        public int BuoySensorEcbPort { get; set; }

        [Required, Range(4, 16)]
        public int BuoySensorEcbPortCount { get; set; }

        [Required, Range(60, 1440)]
        public int BuoyPacketPersistDuration { get; set; }

        [Required, Range(5, 60)]
        public int BuoyPacketEjectionInterval { get; set; }

        //
        //  Will need to be validated against BuoySensorEcbPortCount.
        //
        public Dictionary<int, string> BuoyNames { get; set; } = [];
    }
}

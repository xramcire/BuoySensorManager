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

        [Required, Range(5, 60)]
        public int BuoyPacketPurgeInterval { get; set; }

        [Required, Range(5, 60)]
        public int BuoyPacketRetryInterval { get; set; }

        public Dictionary<int, string> BuoyNames { get; set; } = [];
    }
}

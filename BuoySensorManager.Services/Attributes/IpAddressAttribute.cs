using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace BuoySensorManager.Services.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var input = value as string;

            if (IPAddress.TryParse(input, out var address))
            {
                if (address.AddressFamily != AddressFamily.InterNetwork)
                    return new ValidationResult("Only IPv4 addresses are allowed.");

                return ValidationResult.Success!;
            }

            return new ValidationResult("Invalid IP address format.");
        }
    }

}

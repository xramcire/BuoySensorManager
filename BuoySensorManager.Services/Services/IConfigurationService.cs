using BuoySensorManager.Services.Models;

namespace BuoySensorManager.Services.Services
{
    public interface IConfigurationService
    {
        ConfigRequest Get();

        bool Save(ConfigRequest request);
    }
}
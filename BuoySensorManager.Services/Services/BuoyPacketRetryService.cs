using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Models;
using BuoySensorManager.Core.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace BuoySensorManager.Services.Services
{
    public class BuoyPacketRetryService : BackgroundService
    {
        private readonly IConfig _config;
        private readonly ILogger<BuoyPacketRetryService> _logger;
        private readonly IBuoyPacketRepository _buoyPacketRepository;
        private readonly HttpClient _buoySensorServer;

        public BuoyPacketRetryService(
            IConfig config,
            ILogger<BuoyPacketRetryService> logger,
            IBuoyPacketRepository buoyPacketRepository,
            IHttpClientFactory httpClientFactory
        )
        {
            _config = config;
            _logger = logger;
            _buoyPacketRepository = buoyPacketRepository;
            _buoySensorServer = httpClientFactory.CreateClient("BuoySensorServer");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var retryInterval = _config.BuoyPacketRetryInterval;
                await Task.Delay(TimeSpan.FromMinutes(retryInterval), stoppingToken);
                //
                //  Let start up happen then execute after the interval.
                //
                try
                {

                    await ResendBuoyPackets(stoppingToken);
                }
                catch (Exception ex)
                {
                    //
                    // TODO: How do we recover???
                    //
                    _logger.LogError(ex, "Error in BuoyPacketRetryService");
                }
            }
        }

        private async ValueTask ResendBuoyPackets(CancellationToken stoppingToken)
        {
            //
            //  We could also add a some light weight request to see if the server is available.
            //  Head, Health, Ping, DNS, something we can do without having some data to send.
            //  We could also consider some global flag that tells us if the network connection is up or down.
            //
            long unsent = await _buoyPacketRepository.Count();

            while (unsent > 0)
            {
                var buoyPackets = await _buoyPacketRepository.Fetch(100);

                foreach (var buoyPacket in buoyPackets)
                {
                    //
                    //  TODO: Consider a bulk upload.
                    //
                    bool success = await PostBuoyPacket(buoyPacket, stoppingToken);

                    if (success)
                    {
                        //
                        //  This packet is now safely on the server. Delete it.
                        //
                        await _buoyPacketRepository.Delete(buoyPacket.Id);
                        continue;
                    }
                    //
                    //  If we can't send packets come back and try again later.
                    //
                    break;
                }

                unsent = await _buoyPacketRepository.Count();
            }
        }

        private async ValueTask<bool> PostBuoyPacket(BuoyPacket buoyPacket, CancellationToken cancellationToken)
        {
            try
            {
                //
                //  TODO: How do we measure success. http 200, 201???
                //  TODO: Add a constant for the route.
                //
                //  For testing purposed I am forcing this to succeed.
                await Task.CompletedTask;
                // await PostAsync("api/buoypackets/", buoyPacket, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to send packet to the server.");
                return false;
            }
        }

        private async ValueTask PostAsync(string url, object data, CancellationToken token)
        {
            //
            //  TODO: Authentication...
            //
            string json = JsonSerializer.Serialize(data);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _buoySensorServer.PostAsync(url, requestContent, token);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(token);
                throw new HttpRequestException(responseContent);
            }
        }
    }
}

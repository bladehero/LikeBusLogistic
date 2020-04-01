using LikeBusLogistic.BLL;
using LikeBusLogistic.BLL.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LikeBusLogistic.Web.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly AnonymousServiceFactory _serviceFactory;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, AnonymousServiceFactory serviceFactory)
        {
            _logger = logger;
            _serviceFactory = serviceFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            #region Starting Trips Process

            try
            {
                var trips = _serviceFactory.TripManagement.StartPendingTrips();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            #endregion

            #region MyRegion

            try
            {
                var trips = _serviceFactory.TripManagement.DelayStartedTrips();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            #endregion
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

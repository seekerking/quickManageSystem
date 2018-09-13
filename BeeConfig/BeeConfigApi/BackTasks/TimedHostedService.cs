using BeeConfigApi.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeeConfigApi.BackTasks
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        private readonly BeeConfigService _beeConfigService;

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public TimedHostedService(BeeConfigService configService, ILogger<TimedHostedService> logger)
        {
            _logger = logger;
            _beeConfigService = configService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        public async void DoWork(object state)
        {
            await _beeConfigService.UpdateCache();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}

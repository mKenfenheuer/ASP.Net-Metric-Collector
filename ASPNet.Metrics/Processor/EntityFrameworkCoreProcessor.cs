using KSol.ASPNet.Metrics.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSol.ASPNet.Metrics.Monitoring
{
    internal class EntityFrameworkCoreProcessor : IMetricProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Process(RequestMetric metrics)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            DataBaseContext _dataBaseContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
            await _dataBaseContext.AddAsync(metrics);
            await _dataBaseContext.SaveChangesAsync();
            foreach (MiddlewareMetric m in metrics.MiddlewareMetrics)
            {
                m.RequestMetricId = metrics.Id; 
                m.Id = null;
            }
            await _dataBaseContext.AddRangeAsync(metrics.MiddlewareMetrics);
            await _dataBaseContext.SaveChangesAsync();
            scope.Dispose();
        }
    }
}

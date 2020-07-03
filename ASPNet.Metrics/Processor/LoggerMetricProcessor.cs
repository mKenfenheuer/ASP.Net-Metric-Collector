using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSol.ASPNet.Metrics.Monitoring
{
    internal class LoggerMetricProcessor : IMetricProcessor
    {
        private readonly ILogger<LoggerMetricProcessor> _logger;

        public LoggerMetricProcessor(ILogger<LoggerMetricProcessor> logger)
        {
            _logger = logger;
        }

        public async Task Process(RequestMetric metrics)
        {
            string str = $"Request with Id {metrics.RequestId} finished within {metrics.RequestTimeMs} ms.\n";

            str += $"      Id: {metrics.RequestId}\n";
            str += $"      Path: {metrics.RequestPath}\n";
            str += $"      FullPath: {metrics.RequestFullPath}\n";
            str += $"      Date: {metrics.RequestDate}\n";
            str += $"      ClientIP: {metrics.ClientIP}\n";
            str += $"      ResponseStatusCode: {metrics.ResponseStatusCode}\n";
            str += $"      ResponseContentType: {metrics.ResponseContentType}\n";
            str += $"      ResponseContentLength: {metrics.ResponseContentLength}\n";
            str += $"      RequestContentType: {metrics.RequestContentType}\n";
            str += $"      RequestContentLength: {metrics.RequestContentLength}\n";

            foreach (MiddlewareMetric middlewareMetric in metrics.MiddlewareMetrics)
            {
                str += $"\n      Middleware:\n";
                str += $"      MiddlewareIndex: {middlewareMetric.MiddlewareIndex}\n";
                str += $"      MiddlewareName: {middlewareMetric.MiddlewareName}\n";
                str += $"      MiddlewareTimeMs: {middlewareMetric.MiddlewareTimeMs}\n";

            }

            _logger.LogInformation(str);
        }
    }
}

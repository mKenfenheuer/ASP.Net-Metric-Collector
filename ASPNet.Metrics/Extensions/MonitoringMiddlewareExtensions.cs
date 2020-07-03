using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using Microsoft.AspNetCore.Diagnostics;
using KSol.ASPNet.Metrics.Monitoring;
using System.IO;
using Microsoft.EntityFrameworkCore.Storage;
using KSol.ASPNet.Metrics.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MonitoringMiddlewareExtensions
    {
        public static void AddLoggingMetricProcessor(this IServiceCollection services)
        {
            services.AddTransient<IMetricProcessor, LoggerMetricProcessor>();
        }


        public static void AddEntityFrameworkMetricProcessor(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<DataBaseContext>(options);
            services.AddTransient<IMetricProcessor, EntityFrameworkCoreProcessor>();
        }

        public static void UseRequestMeasuring(this IApplicationBuilder app)
        {
            app.UseMiddleware<MetricsMiddleware>();
        }

        public static void UseMiddlewareMeasuring(this IApplicationBuilder app, string name, Action<IApplicationBuilder> middlewareCreator)
        {
            app.Use(async (context, next) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                int index = (int)(context.Items[$"mmw-index"] ?? 0);
                context.Items[$"mmw-index"] = index + 1;
                stopwatch.Start();
                await next();
                stopwatch.Stop();
                long exclusion = 0;
                if (context.Items.ContainsKey($"{name}-middleware-exclusion-ms"))
                    exclusion = (long)context.Items[$"{name}-middleware-exclusion-ms"];

                long executionTime = stopwatch.ElapsedMilliseconds - exclusion;
                context.Items[$"mmw-{index}-{name}"] = executionTime;

            });
            middlewareCreator(app);
            app.Use(async (context, next) =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await next();
                stopwatch.Stop();
                context.Items[$"{name}-middleware-exclusion-ms"] = stopwatch.ElapsedMilliseconds;
            });
        }
    }
}

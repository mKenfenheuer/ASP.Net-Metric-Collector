using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KSol.ASPNet.Metrics.Monitoring
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMetricProcessor _processor;
        public MetricsMiddleware(RequestDelegate next, IMetricProcessor processor)
        {
            this.next = next;
            this._processor = processor;
        }

        public async Task Invoke(HttpContext context, ILogger<MetricsMiddleware> _logger)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            MemoryStream responseStream = new MemoryStream();
            MemoryStream requestStream = new MemoryStream();
            Stream responseStreamOrig = context.Response.Body;
            Stream requestStreamOrig = context.Request.Body;
            await requestStreamOrig.CopyToAsync(requestStream);
            context.Request.Body = requestStream;
            context.Response.Body = responseStream;
            await next(context);
            responseStream.Position = 0;
            await responseStream.CopyToAsync(responseStreamOrig);
            await responseStreamOrig.FlushAsync();
            context.Response.ContentLength = responseStream.Length;
            context.Request.ContentLength = requestStream.Length;
            stopwatch.Stop();
            List<MiddlewareMetric> metrics = new List<MiddlewareMetric>();
            foreach (object key in context.Items.Keys)
            {
                if (key.ToString().StartsWith("mmw-") && key.ToString() != "mmw-index")
                {
                    string str = key.ToString().Replace("mmw-", "");
                    int index = int.Parse(str.Substring(0, str.IndexOf("-")));
                    string name = str.Replace($"{index}-", "");
                    metrics.Add(new MiddlewareMetric(name, index, (long)context.Items[key]));
                }
            }
            RequestMetric metric = new RequestMetric(
                requestDate: DateTime.UtcNow,
                requestFullPath: $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                requestPath: context.Request.Path,
                requestId: context.TraceIdentifier,
                clientIP: context.Connection.RemoteIpAddress.ToString(),
                requestTimeMs: stopwatch.ElapsedMilliseconds,
                middlewareMetrics: metrics,
                responseStatusCode: context.Response.StatusCode,
                responseContentType: context.Response.ContentType,
                responseContentLength: context.Response.ContentLength ?? 0,
                requestContentType: context.Request.ContentType,
                requestContentLength: context.Request.ContentLength ?? 0               
            );
            new System.Threading.Thread(async () =>
            {
                try
                {
                    await _processor.Process(metric);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while processing metrics:\n{ex}");
                }
            }).Start();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KSol.ASPNet.Metrics.Monitoring
{
    public class MiddlewareMetric
    {
        [ForeignKey(nameof(RequestMetricId))]
        public RequestMetric RequestMetric { get; set; }
        public string RequestMetricId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string MiddlewareName { get; set; }
        public int MiddlewareIndex { get; set; }
        public long MiddlewareTimeMs { get; set; }

        public MiddlewareMetric(string middlewareName, int middlewareIndex, long middlewareTimeMs)
        {
            MiddlewareName = middlewareName;
            MiddlewareIndex = middlewareIndex;
            MiddlewareTimeMs = middlewareTimeMs;
        }

        public MiddlewareMetric(RequestMetric requestMetric, string requestMetricId, string id, string middlewareName, int middlewareIndex, long middlewareTimeMs)
        {
            RequestMetric = requestMetric;
            RequestMetricId = requestMetricId;
            Id = id;
            MiddlewareName = middlewareName;
            MiddlewareIndex = middlewareIndex;
            MiddlewareTimeMs = middlewareTimeMs;
        }

        public MiddlewareMetric()
        {
        }
    }
}

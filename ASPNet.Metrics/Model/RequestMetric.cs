using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KSol.ASPNet.Metrics.Monitoring
{
    public class RequestMetric
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public long RequestTimeMs { get; set; }
        public string RequestId { get; set; }
        public int ResponseStatusCode { get; set; }
        public string RequestPath { get; set; }
        public string RequestFullPath { get; set; }
        public long RequestContentLength { get; set; }
        public long ResponseContentLength { get; set; }
        public string RequestContentType { get; set; }
        public string ResponseContentType { get; set; }
        public string ClientIP { get; set; }
        public DateTime RequestDate { get; set; }
        public List<MiddlewareMetric> MiddlewareMetrics { get; set; }

        public RequestMetric(long requestTimeMs, string requestId, int responseStatusCode, string requestPath, string requestFullPath, long requestContentLength, long responseContentLength, string requestContentType, string responseContentType, string clientIP, DateTime requestDate, List<MiddlewareMetric> middlewareMetrics)
        {
            RequestTimeMs = requestTimeMs;
            RequestId = requestId;
            ResponseStatusCode = responseStatusCode;
            RequestPath = requestPath;
            RequestFullPath = requestFullPath;
            RequestContentLength = requestContentLength;
            ResponseContentLength = responseContentLength;
            RequestContentType = requestContentType;
            ResponseContentType = responseContentType;
            ClientIP = clientIP;
            RequestDate = requestDate;
            MiddlewareMetrics = middlewareMetrics;
        }

        public RequestMetric(string id, long requestTimeMs, string requestId, int responseStatusCode, string requestPath, string requestFullPath, long requestContentLength, long responseContentLength, string requestContentType, string responseContentType, string clientIP, DateTime requestDate, List<MiddlewareMetric> middlewareMetrics)
        {
            Id = id;
            RequestTimeMs = requestTimeMs;
            RequestId = requestId;
            ResponseStatusCode = responseStatusCode;
            RequestPath = requestPath;
            RequestFullPath = requestFullPath;
            RequestContentLength = requestContentLength;
            ResponseContentLength = responseContentLength;
            RequestContentType = requestContentType;
            ResponseContentType = responseContentType;
            ClientIP = clientIP;
            RequestDate = requestDate;
            MiddlewareMetrics = middlewareMetrics;
        }

        public RequestMetric()
        {
        }
    }
}

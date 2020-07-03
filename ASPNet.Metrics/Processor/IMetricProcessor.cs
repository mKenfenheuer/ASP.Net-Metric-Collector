using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSol.ASPNet.Metrics.Monitoring
{
    public interface IMetricProcessor
    {
        Task Process(RequestMetric metrics);
    }
}

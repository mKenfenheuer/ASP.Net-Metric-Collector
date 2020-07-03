using KSol.ASPNet.Metrics.Monitoring;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KSol.ASPNet.Metrics.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext (DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        internal DbSet<RequestMetric> RequestMetrics { get; set; }
        internal DbSet<MiddlewareMetric> MiddlewareMetrics { get; set; }
    }
}

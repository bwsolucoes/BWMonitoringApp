using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWMonitoringApp;

public class DatabaseCleanup
{
    private readonly ILiteCollection<SystemMetrics> _col;

    public DatabaseCleanup(ILiteCollection<SystemMetrics> col)
    {
        _col=col;
    }
    public void PurgeOldMetrics(int daysToKeep)
    {
        var cutoff = DateTime.UtcNow.AddDays(-daysToKeep);
        _col.DeleteMany(m=>m.MetricDate < cutoff);
    }
}

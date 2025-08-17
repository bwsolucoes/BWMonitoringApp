using System.Diagnostics;
using System.Management;
using System.Net;
using System.Text;
using LiteDB;
using Newtonsoft.Json;

namespace BWMonitoringApp;

internal class Program
{
    private static void Main(string[] args)
    {
        const string databaseName = "MetricsDB";
        using var db = new LiteDatabase(databaseName);

        var registers = db.GetCollection<SystemMetrics>("registros");
        var register = new SystemMetrics();

        SystemMetrics metrics = new SystemMetrics();
        MetricsCollector metricsCollector = new MetricsCollector();
        metricsCollector.Collect();
        metricsCollector.PrintMetrics();

        registers
    }
}

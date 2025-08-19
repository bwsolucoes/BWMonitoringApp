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
        for(int i = 0; i <=5; i++)
        {
            SystemMetrics metrics = new SystemMetrics();
            MetricsCollector metricsCollector = new MetricsCollector();
            register = metricsCollector.Collect();


            registers.Insert(register);
            Thread.Sleep(10000);
        }
        
    }
}

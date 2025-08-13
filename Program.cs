using System.Diagnostics;
using System.Management;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace BWMonitoringApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var metricsCollector = new MetricsCollector();
        metricsCollector.Collect();
        metricsCollector.PrintMetrics();
    }
}

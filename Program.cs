using LiteDB;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BWMonitoringApp;

internal class Program
{
    private static string _datadogApiKey = "";
    private static string _datadogApiUrl = "";
    private static async Task Main(string[] args)
    {
        const string databaseName = "MetricsDB";
        using var db = new LiteDatabase(databaseName);

        var registers = db.GetCollection<SystemMetrics>("registros");
        var register = new SystemMetrics();

        _datadogApiKey = "";
        _datadogApiUrl = $"https://api.datadoghq.com/api/v1/series";
        DataSender _dataSender = new DataSender(_datadogApiUrl);

        for(int i = 0; i <=10; i++)
        {
            SystemMetrics metrics = new SystemMetrics();
            MetricsCollector metricsCollector = new MetricsCollector();
            register = metricsCollector.Collect();


            registers.Insert(register);
            await _dataSender.SendToDatadog(register);

            new DatabaseCleanup(registers).PurgeOldMetrics(1);
            Thread.Sleep(10000);
        }
        
    }
}

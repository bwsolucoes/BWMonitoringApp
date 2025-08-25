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
        _datadogApiUrl = $"https://{url}/api/v1/series?api_key={datadogApiKey}";
        DataSender _dataSender = new DataSender()
        for(int i = 0; i <=5; i++)
        {
            SystemMetrics metrics = new SystemMetrics();
            MetricsCollector metricsCollector = new MetricsCollector();
            register = metricsCollector.Collect();


            registers.Insert(register);

            
            new DatabaseCleanup(registers).PurgeOldMetrics(1);
            Thread.Sleep(10000);
        }
        
    }
}

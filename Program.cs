using LiteDB;
using Microsoft.Extensions.Configuration;
using DotNetEnv;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetEnv.Configuration;

namespace BWMonitoringApp;

internal class Program
{
    private static string _datadogApiKey = "";
    private static string _datadogApiUrl = "";
    private static async Task Main(string[] args)
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appFolder = Path.Combine(localAppData, "BW Endpoint Monitor");

        if (!Directory.Exists(appFolder))
            Directory.CreateDirectory(appFolder);
        
        const string databaseName = "MetricsDB";
        string dbPath = Path.Combine(appFolder, databaseName);
        using var db = new LiteDatabase(dbPath);

        IConfiguration config = new ConfigurationBuilder().
                                    AddUserSecrets<Program>().
                                    AddDotNetEnv().
                                    Build();

        ConfigInfo configInfo = new ConfigInfo();
        AuthManager authManager = new AuthManager(configInfo, config);
        authManager.GetConfigInfo();
        
        ILiteCollection<SystemMetrics> registers = db.GetCollection<SystemMetrics>("registros");
        SystemMetrics register = new SystemMetrics();

        _datadogApiKey = "";
        _datadogApiUrl = $"https://app.datadoghq.com/api/v1/series";
        DataSender _dataSender = new DataSender(configInfo);

        while(true)
        {
            SystemMetrics metrics = new SystemMetrics();
            MetricsCollector metricsCollector = new MetricsCollector();
            register = metricsCollector.Collect();


            registers.Insert(register); 
            await _dataSender.SendToDatadog(register);

            new DatabaseCleanup(registers).PurgeOldMetrics(1);
            Thread.Sleep(120000);
        }
        
    }
}

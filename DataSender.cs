using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWMonitoringApp;

public class DataSender
{
    private readonly string _apiUrl;
    private readonly HttpClient _httpClient;
    public DataSender(string apiUrl)
    {
        _apiUrl = apiUrl;
        _httpClient = new HttpClient();
    }

    public async Task SendToDatadog(SystemMetrics metrics)
    {
        var tags = new[] { $"host:{metrics.Hostname}" ,$"username:{metrics.Username}"};
    }


}

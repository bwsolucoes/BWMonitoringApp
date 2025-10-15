using Newtonsoft.Json;
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
    private readonly string _apiKey;
    private readonly string _appKey;
    private readonly ConfigInfo _info;
    public DataSender(ConfigInfo info)
    {
        _info = info;

        _apiKey = _info.ApiKey;
        _appKey = _info.AppKey;
        _apiUrl = $"https://{_info.DatadogUrl}/api/v1/series";
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("DD-API-KEY", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("DD-APP-KEY", _appKey);
    }

    public async Task SendToDatadog(SystemMetrics metrics)
    {
        string[] tags = [$"host:{metrics.Hostname}" ,$"username:{metrics.Username}"];
        long timestamp = ((DateTimeOffset)metrics.MetricDate).ToUnixTimeSeconds();
        var series = new List<object>
        {
            MetricToObject("monitor.endpoint.system.cpu.usage", metrics.CpuUsage, timestamp, tags),
            MetricToObject("monitor.endpoint.system.memory.available", metrics.AvailableRam, timestamp, tags),
            MetricToObject("monitor.endpoint.system.disk.usage", metrics.DiskUsage, timestamp, tags),
            MetricToObject("monitor.endpoint.system.disk.reads", metrics.DiskReads, timestamp, tags),
            MetricToObject("monitor.endpoint.system.disk.writes", metrics.DiskWrites, timestamp, tags),
            MetricToObject("monitor.endpoint.system.disk.free", metrics.AvailableDisk, timestamp, tags),
            MetricToObject("monitor.endpoint.system.network.usage", metrics.NetworkUsage, timestamp, tags),
            MetricToObject("monitor.endpoint.system.uptime", metrics.Uptime, timestamp, tags),
        };
        
        
        var payload = new
        {
            series
        };

        string json = JsonConvert.SerializeObject(payload, Formatting.Indented);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Metrics sent to Datadog successfully.\n");
        }
        else
        {
            Console.WriteLine($"Failed to send metrics to Datadog: {response.StatusCode}");
        }
    }

    private static object MetricToObject(string name, float value, long timestamp, string[] tags)
    {
        return new
        {
            metric = name,
            points = new[] {new[] { timestamp, value } },
            type = "gauge",
            tags = tags
        };
    }
}

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
    public DataSender(string apiUrl)
    {
        _apiUrl = apiUrl;
        _httpClient = new HttpClient();
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
            MetricToObject("monitor.endpoint.system.disk.free", metrics.AvailableDisk, timestamp, tags),
            MetricToObject("monitor.endpoint.system.network.usage", metrics.CpuUsage, timestamp, tags),
        };

        string json = JsonConvert.SerializeObject(series);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Metrics sent to Datadog successfully.");
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
            points = new[] { timestamp, value },
            type = "gauge",
            tags
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWMonitoringApp;

public class SystemMetrics
{
    public string Hostname { get; set; }
    public string Username { get; set; }
    public float CpuUsage { get; set; }
    public float TotalPhysMemory { get; set; }
    public float AvailableRam { get; set; }
    public float DiskUsage { get; set; }
    public float AvailableDisk { get; set; }
    public float NetworkUsage { get; set; }
    public DateTime MetricDate { get; set; }
    public float Uptime { get; set; }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BWMonitoringApp;

public class MetricsCollector
{
    private static readonly string hostname = Dns.GetHostName();
    private static readonly string username = Environment.UserName;

    private static float cpuUsage;
    private static float availableMemory;
    private static float diskUsage;
    private static float networkUsage;
    private static long totalDiskSpace;
    private static long usedDiskUsage;
    private static long freeDiskSpace;

    private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    private static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
    private static PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
    private static PerformanceCounter networkCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", GetNetworkInterface());
    
    public SystemMetrics Collect()
    {
        cpuUsage = GetCpuCounter(cpuCounter);
        availableMemory = GetRamCounter(ramCounter);
        diskUsage = GetDiskUsage(diskCounter);
        networkUsage = GetNetworkUsage(networkCounter);
        freeDiskSpace = GetDiskSpace();

        PrintMetrics();
        return new SystemMetrics
        {
            CpuUsage = cpuUsage,
            AvailableRam = availableMemory,
            DiskUsage = diskUsage,
            NetworkUsage = networkUsage,
            AvailableDisk = freeDiskSpace,
            MetricDate = DateTime.Now
        };
    }

    public void PrintMetrics()
    {
        Console.WriteLine($"Hostname: {hostname}");
        Console.WriteLine($"Username: {username}");
        Console.WriteLine($"CPU Usage: {cpuUsage}%");
        Console.WriteLine($"Available Memory: {availableMemory} MB");
        Console.WriteLine($"Disk Usage: {diskUsage}%");
        Console.WriteLine($"Network Usage: {networkUsage} Bytes/sec");
        //Console.WriteLine($"Total Disk Space: {totalDiskSpace / (1024 * 1024 * 1024)} GB");
        Console.WriteLine($"Free Disk Space: {freeDiskSpace / (1024 * 1024 * 1024)} GB");
    }

    #region Getters
    private static string GetNetworkInterface()
    {
        var category = new PerformanceCounterCategory("Network Interface");
        string[] instanceNames = category.GetInstanceNames();

        return instanceNames.Length > 0 ? instanceNames[0] : null;
    }

    private static float GetCpuCounter(PerformanceCounter cpuCounter)
    {
        float usage;
        cpuCounter.NextValue();
        Thread.Sleep(1000);
        usage = cpuCounter.NextValue();
        
        return usage;
    }

    private static float GetRamCounter(PerformanceCounter ramCounter)
    {
        float available = ramCounter.NextValue();

        return available;
    }

    private static float GetDiskUsage(PerformanceCounter diskCounter)
    {
        float usage;
        diskCounter.NextValue();
        Thread.Sleep(1000);
        usage = diskCounter.NextValue();

        return usage;
    }

    private static long GetDiskSpace()
    {
        DriveInfo driveInfo = new DriveInfo("C");
        long totalDiskSpace = driveInfo.TotalSize;
        long freeDiskSpace = driveInfo.AvailableFreeSpace;
        long usedDiskSpace = totalDiskSpace - freeDiskSpace;

        return freeDiskSpace;
    }

    private static float GetNetworkUsage(PerformanceCounter networkCounter)
    {
        float usage = networkCounter.NextValue();
        return usage;
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

namespace BWMonitoringApp;

public class MetricsCollector
{
    private static readonly string hostname = Dns.GetHostName();
    private static readonly string username = Environment.UserName;

    private static readonly string? _mainDisk = GetInstallationDisk() ?? "_Total";

    private static float cpuUsage;
    private static float totalPhysMemory;
    private static float availableMemory;
    private static float diskUsage;
    private static float diskReads;
    private static float diskWrites;
    private static float networkUsage;
    private static long totalDiskSpace;
    private static long usedDiskUsage;
    private static long freeDiskSpace;
    private static float uptime;

    private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    private static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
    private static PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", _mainDisk);
    private static PerformanceCounter diskReadCounter = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", _mainDisk);
    private static PerformanceCounter diskWriteCounter = new PerformanceCounter("PhysicalDisk", "Disk Writes/sec", _mainDisk);
    private static PerformanceCounter networkCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", GetNetworkInterface());
    private static ComputerInfo computerInfo = new ComputerInfo();

    public SystemMetrics Collect()
    {
        cpuUsage = GetCpuCounter(cpuCounter);
        availableMemory = GetRamCounter(ramCounter);
        totalPhysMemory = GetTotalRam(computerInfo);
        diskUsage = GetDiskUsage(diskCounter);
        diskReads = GetDiskReads(diskReadCounter);
        diskWrites = GetDiskWrites(diskWriteCounter);
        networkUsage = GetNetworkUsage(networkCounter);
        freeDiskSpace = GetDiskSpace();
        uptime = GetUptime();
        PrintMetrics();
        return new SystemMetrics
        {
            Hostname = hostname,
            Username = username,
            CpuUsage = cpuUsage,
            TotalPhysMemory = totalPhysMemory,
            AvailableRam = availableMemory,
            DiskUsage = diskUsage,
            DiskReads = diskReads,
            DiskWrites = diskWrites,
            NetworkUsage = networkUsage,
            AvailableDisk = freeDiskSpace,
            MetricDate = DateTime.Now,
            Uptime = uptime
        };
    }

    public void PrintMetrics()
    {
        Console.WriteLine($"Hostname: {hostname}");
        Console.WriteLine($"Username: {username}");
        Console.WriteLine($"CPU Usage: {cpuUsage}%");
        Console.WriteLine($"Total Physical Memory: {totalPhysMemory / (1024 * 1024)} MB");
        Console.WriteLine($"Available Memory: {availableMemory} MB");
        Console.WriteLine($"Disk Usage: {diskUsage}%");
        Console.WriteLine($"Disk Reads: {diskReads}");
        Console.WriteLine($"Disk Writes: {diskWrites}");
        Console.WriteLine($"Disk Read/Write: {diskReads / diskWrites}");
        Console.WriteLine($"Network Usage: {networkUsage} Bytes/sec");
        //Console.WriteLine($"Total Disk Space: {totalDiskSpace / (1024 * 1024 * 1024)} GB");
        Console.WriteLine($"Free Disk Space: {freeDiskSpace / (1024 * 1024 * 1024)} GB");
        Console.WriteLine($"Machine Uptime: {uptime / 86400} days");
    }

    #region Getters
    private static string? GetNetworkInterface()
    {
        var category = new PerformanceCounterCategory("Network Interface");
        string[] instanceNames = category.GetInstanceNames();

        return instanceNames.Length > 0 ? instanceNames[0] : null;
    }
    private static string GetRootDir()
    {
        string _rootDir = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        return _rootDir; 
    }
    private static string? GetInstallationDisk()
    {
        string? dir = GetRootDir();
        if (string.IsNullOrEmpty(dir))
            return "_Total";

        string drive = dir.TrimEnd('\\');

        PerformanceCounterCategory category = new PerformanceCounterCategory("PhysicalDisk");
        string[]? instances = category.GetInstanceNames();
        string matchDrive = instances.FirstOrDefault(inst => inst.Contains(drive, StringComparison.OrdinalIgnoreCase));
        
        if(matchDrive != null)
        {
            return matchDrive;
        }

        return "_Total";
    }
    private static float GetDiskReads(PerformanceCounter readCounter)
    {
        readCounter.NextValue();
        Thread.Sleep(1000);
        float reads = readCounter.NextValue();
        return reads;
    }
    private static float GetDiskWrites(PerformanceCounter writeCounter)
    {
        writeCounter.NextValue();
        Thread.Sleep(1000);
        float writes = writeCounter.NextValue();
        return writes;
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

    private static float GetTotalRam(ComputerInfo ci)
    {
        // ulong bla = computerInfo.AvailablePhysicalMemory;
        ulong physMemory = ci.TotalPhysicalMemory;
        return (float)physMemory;
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
        string drive = GetRootDir();
        string _trimDrive = drive.Trim([':','\\']);
        DriveInfo driveInfo = new DriveInfo(_trimDrive);
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

    private static float GetUptime()
    {
        long ticks = Stopwatch.GetTimestamp();
        double uptime = ((double)ticks) / Stopwatch.Frequency;
        return (float)uptime;
        //TimeSpan uptimeSpan = TimeSpan.FromSeconds(uptime);
        
    }
    #endregion
}

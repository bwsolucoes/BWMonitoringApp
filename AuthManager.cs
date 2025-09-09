using System;
using System.IO;
using DotNetEnv;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BWMonitoringApp;

public sealed class AuthManager
{
    private static readonly string _envPath = ".env";

    private static ConfigInfo _info = new ConfigInfo();
    private static IConfiguration _config;

    public AuthManager(ConfigInfo info, IConfiguration config)
    {
        _info = info;
        _config = config;
    }

    private void FileExists()
    {
        if (!File.Exists(_envPath))
        {
            File.Create(_envPath).Close();
        }
        DotNetEnv.Env.Load();
    }

    public ConfigInfo GetConfigInfo()
    {
        FileExists();

        _info.ApiKey = _config["Datadog:ApiKey"] ?? DotNetEnv.Env.GetString("API_KEY") ?? SaveKeyOnEnv("API_KEY");
        _info.AppKey = _config["Datadog:AppKey"] ?? DotNetEnv.Env.GetString("APP_KEY") ?? SaveKeyOnEnv("APP_KEY");
        _info.DatadogUrl = _config["Datadog:Url"] ?? DotNetEnv.Env.GetString("DD_URL") ??  SaveKeyOnEnv("DD_URL");

        return _info;
    }

    private string SaveKeyOnEnv(string key)
    {
        Console.WriteLine($"{key} is missing from the file, please insert you {key}: ");
        string aux = "";
        aux = Console.ReadLine();

        while (string.IsNullOrEmpty(aux))
        {
            Console.WriteLine($"Invalid insertion. Please write your {key}.");
            aux = Console.ReadLine();
        }

        File.AppendAllText(_envPath, Environment.NewLine + $"{key}={aux}"  );

        //TODO: Adicionar verificação das URLs do Datadog
        return aux;
    }
}
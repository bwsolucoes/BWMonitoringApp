using System;
using System.IO;
using DotNetEnv;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWMonitoringApp;

public sealed class AuthManager
{
    private static readonly string _envPath = ".env";

    private static string _apiKey;
    private static string _appKey;
    private static string _datadogUrl;
    private static ConfigInfo _info;

    public AuthManager(ConfigInfo info)
    {
        _info = info;
    }
    private void FileExists()
    {
        Console.WriteLine("Verificando se o env existe");
        if (!File.Exists(_envPath)) 
        {
            Console.WriteLine("env no existe, criei um pra tu\n");
            File.Create(_envPath).Close();
        }
        DotNetEnv.Env.Load();
    }

    public ConfigInfo GetConfigInfo() 
    {
        FileExists();
        Console.WriteLine("Vendo as linha do arquivo\n");
        string[] lines = File.ReadAllLines(_envPath);

        _info.ApiKey = DotNetEnv.Env.GetString("API_KEY") ?? Write();
        _info.AppKey = DotNetEnv.Env.GetString("APP_KEY") ?? Write();
        _info.AppKey = Env.GetString("DD_URL") ?? Write();

        return _info;
    }

    private string Write()
    {
        Console.WriteLine("Aoba\n");
        throw new NotImplementedException();
    }
}

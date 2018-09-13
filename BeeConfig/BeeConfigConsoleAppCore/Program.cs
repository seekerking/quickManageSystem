using BeeConfigClientStandard;
using System;
using System.Threading;

namespace BeeConfigConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            BeeConfigManager.Init("http://localhost:54774/api/beeconfig/getupdate", "BeeConfig_Test_Console", "TEST", "cda75e2f-f2bf-4b11-90ec-d0b5d8833a8e", "BeeConfig.json", 10000);
            while (true)
            {
                
                ConsoleLog(BeeConfigManager.GetBeeConfig("ConStr"));
                ConsoleLog(BeeConfigManager.GetBeeConfig("Redis-Timeout"));
                ConsoleLog(BeeConfigManager.GetBeeConfig("AppName"));
                Thread.Sleep(10000);
            }
        }

        static void ConsoleLog(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} _ {message}");
        }
    }
}

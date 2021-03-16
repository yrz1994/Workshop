using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Winton.Extensions.Configuration.Consul;

namespace ServiceClientA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        //使用consul客户端加载consul配置
                        config.AddConsul("Client.Json", options =>
                        {
                            options.ConsulConfigurationOptions = cco =>
                            {
                                cco.Address = new Uri("http://localhost:8500");
                            };
                            //配置热更新 动态加载
                            options.ReloadOnChange = true;
                            options.Optional = true;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}

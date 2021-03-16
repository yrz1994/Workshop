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
                        //ʹ��consul�ͻ��˼���consul����
                        config.AddConsul("Client.Json", options =>
                        {
                            options.ConsulConfigurationOptions = cco =>
                            {
                                cco.Address = new Uri("http://localhost:8500");
                            };
                            //�����ȸ��� ��̬����
                            options.ReloadOnChange = true;
                            options.Optional = true;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}

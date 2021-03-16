using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace ServiceA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceA", Version = "v1" });
            });
            services.AddSingleton<IConsulClient>(sp =>
            {
                return new ConsulClient(c => { c.Address = new Uri("http://localhost:8500"); });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IConsulClient consulClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceA v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var id = Guid.NewGuid();
            var serviceName = "ServiceA";
            int.TryParse(Configuration["port"], out var port);  //端口从命令行参数中获取
            lifetime.ApplicationStarted.Register(() =>
            {
                var healthCheck = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30),//注销时间
                    Interval = TimeSpan.FromSeconds(5),    //健康检测间隔
                    HTTP = $"http://localhost:{port}/api/health",
                    Timeout = TimeSpan.FromSeconds(5),      //超时时间
                    TLSSkipVerify = true
                };

                var registration = new AgentServiceRegistration
                {
                    Checks = new[] { healthCheck },
                    ID = $"{serviceName}_{id}",
                    Name = serviceName,
                    Address = "localhost",
                    Port = port,
                    Tags = null
                };
                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister($"{serviceName}_{id}").Wait();
            });
        }
    }
}

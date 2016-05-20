using Lohmann.HALight.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lohmann.HALight.Sample
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void ConfigureServices(
            IServiceCollection services)
        {
            var logger = _loggerFactory.CreateLogger<HalInputFormatter>();
            services.AddMvc(options =>
            {                
                options.InputFormatters.Add(new HalInputFormatter(logger));
                options.OutputFormatters.Add(new HalOutputFormatter());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {            
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
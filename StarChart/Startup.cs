using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarChart.Data;

namespace StarChart
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC to the middleware pipeline.
            services.AddMvc();

            // Setup EntityFramework DB context.
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("StarChart"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add MVC to the application pipeline.
            app.UseMvc();
        }
    }
}

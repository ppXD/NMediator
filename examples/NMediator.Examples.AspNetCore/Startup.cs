using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NMediator.Examples.AspNetCore.CommandHandlers;
using NMediator.Examples.AspNetCore.Middlewares;

namespace NMediator.Examples.AspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var mediator = new MediatorConfiguration()
                .RegisterHandler<TestCommandHandlers>()
                // .UseMiddleware<TestMiddleware>()
                // .UseMiddleware(next =>
                // {
                //     return async message =>
                //     {
                //         await next(message);
                //     };
                // })
                .CreateMediator();

            services.AddScoped(x => mediator);
            services.AddMvc(opt =>
            {
                opt.Filters.Add<TestFilter1>();
                opt.Filters.Add<TestFilter2>();
                opt.Filters.Add<TestFilter3>();
                opt.Filters.Add<ExceptionFilter1>();
                opt.Filters.Add<ExceptionFilter2>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TestMiddleware1>();
            app.UseMiddleware<TestMiddleware2>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
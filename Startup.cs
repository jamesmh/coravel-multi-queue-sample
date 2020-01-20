using Coravel;
using Coravel.Queuing.Interfaces;
using coravelMultiQueueSample.Events;
using coravelMultiQueueSample.Invocables;
using coravelMultiQueueSample.Listeners;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace coravelMultiQueueSample
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
            services.AddControllersWithViews();

            services.AddScheduler();
            services.AddQueue();
            services.AddEvents();
            services.AddTransient<InvocableOne>();
            services.AddTransient<InvocableTwo>();
            services.AddTransient<InvocableThree>();
            services.AddTransient<ListenerOne>();
            services.AddTransient<ListenerTwo>();
            services.AddTransient<ListenerThree>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.ApplicationServices.ConfigureEvents()
                .Register<TestEvent>()
                    .Subscribe<ListenerOne>()
                    .Subscribe<ListenerTwo>()
                    .Subscribe<ListenerThree>();

            app.ApplicationServices.UseScheduler(s => {
                // Trigger broadcast event with multiple listeners every ten seconds.
                s.Schedule(() => {
                    using(var scope = app.ApplicationServices.CreateScope())
                    {
                        var queue = scope.ServiceProvider.GetRequiredService<IQueue>();
                        queue.QueueBroadcast(new TestEvent());
                    }
                }).EveryTenSeconds();

                // Trigger queing multiple invocables every ten seconds.
                s.Schedule(() => {
                    using(var scope = app.ApplicationServices.CreateScope())
                    {
                        var queue = scope.ServiceProvider.GetRequiredService<IQueue>();
                        queue.QueueInvocable<InvocableOne>();
                        queue.QueueInvocable<InvocableTwo>();
                        queue.QueueInvocable<InvocableThree>();
                    }
                }).EveryTenSeconds();
            });
        }
    }
}

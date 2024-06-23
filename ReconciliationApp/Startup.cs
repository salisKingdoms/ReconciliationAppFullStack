using Microsoft.AspNetCore.Http.Features;
using ReconciliationApp.Data.DataAccess;
using ReconciliationApp.Data.Repository;

namespace ReconciliationApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddMvc().AddControllersAsServices();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddScoped<IsqlDataAccess, sqlDataAccess>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<ITransRepo, TransRepo>();

        }


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



            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                                  name: "default",
                                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

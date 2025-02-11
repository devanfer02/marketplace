using Marketplace.Infra.Configuration;
using Marketplace.Infra.Database;
using Marketplace.Packages.HostedServices;
using Marketplace.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infra.Server
{
    public class HttpServer
    {
        private static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<AppConfiguration>(builder.Configuration);
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews().AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/auth/login";
                options.AccessDeniedPath = "/";
            });

            var appSettings = builder.Configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>()!;

            builder.Services.AddDbContext<EFDBAccess>(options => options.UseNpgsql(appSettings.Database.DefaultConnection));

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddHostedService<TailwindHostedService>();
            }
            
            // register for DI
            builder.Services.AddScoped<IUserRepository, EFUserRepository>();
            builder.Services.AddScoped<IProductRepository, EFProductRepository>();

            return builder;
        }


        public static void StartWebApplication(string[] args)
        {
            var app = CreateBuilder(args).Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbAccess = scope.ServiceProvider.GetRequiredService<EFDBAccess>();
                dbAccess.Database.EnsureCreated();
                dbAccess.Database.Migrate();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();

        }
    }
}

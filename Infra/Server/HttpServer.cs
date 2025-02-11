using Marketplace.Infra.Configuration;
using Marketplace.Infra.Database;
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
            builder.Services.AddControllersWithViews();

            var appSettings = builder.Configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>()!;

            builder.Services.AddDbContext<EFDBAccess>(options => options.UseNpgsql(appSettings.Database.DefaultConnection));

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
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();

        }
    }
}

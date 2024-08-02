using Data_Access_Layer.DBContext;
using Data_Access_Layer.Interfaces;
using Data_Access_Layer.Repository;
using Microsoft.EntityFrameworkCore;
using PicciLeaksModels;

namespace PicciLeaks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PicciLeaksSqlContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PicciLeaksConnection")));

            builder.Services.AddScoped<IDbActions<Picture>, SqlDB>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.Services.GetRequiredService<IWebHostEnvironment>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Gallery}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
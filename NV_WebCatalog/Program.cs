using NV_WebCatalog.Application.Services;
using NV_WebCatalog.Infrastructure.Repositories;

namespace NV_WebCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Services to Container
            builder.Services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            // Repository Layer
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // Service Layer
            builder.Services.AddScoped<IProductService, ProductService>();

            // Build App
            var app = builder.Build();

            // Configure HTTP Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

global using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Interfaces;
using NewPortfolio.Models;
using NewPortfolio.Repositories;

namespace NewPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IGame,GameRepository>();
            builder.Services.AddTransient<IGenre,GenreRepository>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();


            //For production lifetime
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("lazaruswill.cz");
                options.ExcludedHosts.Add("www.lazaruswill.cz");
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            //using (var scope = app.Services.CreateScope())
            //{
            //    RoleManager<IdentityRole> spravceRoli = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    UserManager<AppUser> spravceUzivatelu = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            //    spravceRoli.CreateAsync(new IdentityRole("admin")).Wait();
            //    AppUser uzivatel = spravceUzivatelu.FindByEmailAsync("admin@seznam.cz").Result;
            //    spravceUzivatelu.AddToRoleAsync(uzivatel, "admin").Wait();

            //    //spravceRoli.CreateAsync(new IdentityRole("admin")).Wait();
            //    //AppUser uzivatel2 = spravceUzivatelu.FindByEmailAsync("petr.valosek@post.cz").Result;
            //    //spravceUzivatelu.AddToRoleAsync(uzivatel2, "admin").Wait();

            //}
            app.Run();
        }
    }
}
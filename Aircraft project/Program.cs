using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aircraft_project.Data;
using Aircraft_project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DinkToPdf.Contracts;
using DinkToPdf;
using System;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUserService, UserService>();

/*builder.Services.AddSingleton<IPdfGenerator, PdfGenerator>();
builder.Services.AddScoped<IUserService, UserService>();*/


// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.LoginPath = "/Client/Login";
        options.LogoutPath = "/Client/Logout";
        options.SlidingExpiration = true;
    });


//  ApplicationDbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





var app = builder.Build();




app.UseSession(); // session middleware



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();  // Use authorization middleware

app.UseAuthorization();


// Routing for Client & Dashboard
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Client}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "dashboard/{action=Index}/{id?}",
        defaults: new { controller = "Dashboard", action = "Index" });
});

app.Run();
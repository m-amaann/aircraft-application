using Aircraft_project.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; //cookies
});


// Register the ApplicationDbContext in the DI container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();




app.UseSession();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// Routing Set Up for Client & Admin
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard/{action=Index}/{id?}",
    defaults: new { controller = "Dashboard" });


app.Run();




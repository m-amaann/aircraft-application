var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// Default route to Client
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard/{action=Index}/{id?}",
    defaults: new { controller = "Dashboard" });


app.Run();

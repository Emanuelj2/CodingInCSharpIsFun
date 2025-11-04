using Microsoft.EntityFrameworkCore;
using MVCApplication.Data;
using MVCApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//connect to the database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "products",
    pattern: "products/{action=Index}/{id?}",
    defaults: new { controller = "Product" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "invoices",
    pattern: "invoices/{action=Index}/{id?}",
    defaults: new { controller = "Invoice" })
    .WithStaticAssets();

app.Run();

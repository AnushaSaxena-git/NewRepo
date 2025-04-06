using Catlog.DTO;
using Catlog.Models;
using Catlog.Repositories;
using Catlog.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Ensure cookie security
    options.Cookie.IsEssential = true; // Make session cookie essential
});
// Add services to the container.
builder.Services.AddControllersWithViews();
// Add services for MVC and Swagger
builder.Services.AddControllersWithViews(); // For MVC views
builder.Services.AddEndpointsApiExplorer(); // For API endpoint exploration

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(ProductProfile)); // Register AutoMapper



var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<CatlogDBcontext>(options =>

options.UseSqlServer(config.GetConnectionString("dbcs")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductSkuRepository, ProductskuRepo>();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<Ishippingmodelrepository, ShippingModelRepository>();
builder.Services.AddScoped<IProduct_ImageRepository, Product_ImageRepository>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
//builder.Services.AddScoped<IShippingTypeRepository, IShippingTypeRepository>();  



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
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=InsertProductAndSku}/{id?}");
//app.MapControllerRoute(
//name: "default",
//    pattern: "{controller=Home}/{action=CreateShippingType}/{id?}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

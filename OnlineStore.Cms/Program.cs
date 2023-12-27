using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Cms.Extensions;
using OnlineStore.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryDbConnection"), options => options.CommandTimeout(360));
});
Account cloudinaryCredentials = new Account(
    builder.Configuration["Cloudinary:CloudName"],
    builder.Configuration["Cloudinary:ApiKey"],
    builder.Configuration["Cloudinary:ApiSecret"]);

Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

builder.Services.AddSingleton(cloudinaryUtility);
builder.Services.AddControllersWithViews();
builder.Services.RegisterService();
builder.Services.AddLogging();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using BestStore.Data;
using BestStore.Models;
using BestStore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Prometheus; 

using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Chỉ định kết nối phù hợp (Docker hoặc Local)
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var connectionString = isDocker
    ? builder.Configuration.GetConnectionString("DockerConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

// Gọi AddDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

builder.Services.AddScoped<EmployeeService>();

//builder.WebHost.UseUrls("http://0.0.0.0:80");

// Cấu hình Identity
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Auto migrate khi khởi động (dùng cho Dev/Test)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // hoặc db.Database.EnsureCreated();
}

// Middleware cho lỗi và HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();// Bật HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//  Prometheus metrics middleware
app.UseHttpMetrics(); //  Ghi log metrics cho toàn bộ HTTP request

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapMetrics(); // Prometheus metrics tại /metrics
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

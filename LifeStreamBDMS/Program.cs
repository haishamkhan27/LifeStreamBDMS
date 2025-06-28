using LifeStreamBDMS.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Required for session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session expiration time
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); // Enables session state
builder.Services.AddHttpContextAccessor(); // Required for accessing session in controllers

// 🔹 Database Context Setup
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Authentication & Authorization setup
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect unauthorized users to login
        options.AccessDeniedPath = "/Home/AccessDenied"; // Redirect unauthorized access attempts
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 🔹 Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Enables detailed error messages
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 🔹 Ensure session and authentication middleware are properly initialized
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// 🔹 Corrected routing for viewing donors
app.MapControllerRoute(
    name: "viewDonors",
    pattern: "Donor/ViewRegisteredDonors",
    defaults: new { controller = "Donor", action = "ViewRegisteredDonors" });

// 🔹 Corrected routing for blood requests
app.MapControllerRoute(
    name: "viewBloodRequests",
    pattern: "BloodRequest/AllRequests",
    defaults: new { controller = "BloodRequest", action = "AllRequests" });

// 🔹 Role-based routing for admin actions
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Dashboard}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute( 
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "viewPendingRequests",
    pattern: "BloodRequest/PendingRequests",
    defaults: new { controller = "BloodRequest", action = "PendingRequests" });


app.Run();

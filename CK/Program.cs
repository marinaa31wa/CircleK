using CK.Model;
using CK.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FluentAssertions.Common; // Consider removing this line if FluentAssertions.Common is not being used
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers, HTTP context accessor, MVC, authentication, session, etc.
// ...
builder.Services.AddControllersWithViews();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//builder.Services.AddDbContext<DataCenterContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
//        sqlServerOptionsAction: sqlOptions =>
//        {
//            sqlOptions.EnableRetryOnFailure(
//                maxRetryCount: 10,
//                maxRetryDelay: TimeSpan.FromSeconds(30),
//                errorNumbersToAdd: null);
//        });
//});
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(option =>
//    {
//        option.LoginPath = "/Login/Login";
//        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//    });
builder.Services.AddDbContext<CkhelperdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set an appropriate timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

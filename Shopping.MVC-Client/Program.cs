using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Shopping.Core.Models;
using Shopping.Core.Services;
using Shopping.MVC_Client.Handler;
using Shopping.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));
builder.Services.AddHttpContextAccessor();

//IClientAccessTokenCache dependency inject ede bimek ucun asagidaki kod istifade olunur
builder.Services.AddAccessTokenManagement();

builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
{
    opts.LoginPath = "/Auth/SignIn";
    opts.ExpireTimeSpan = TimeSpan.FromDays(60);
    opts.SlidingExpiration = true;
    opts.Cookie.Name = "webcookie";
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

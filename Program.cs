using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using FileUploader.Models;
using FileUploader.Repositories.Contracts;
using FileUploader.Repositories;
using FileUploader.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddScoped<IFileRepository, FileRespository>();
builder.Services.AddSingleton<IAws3Services, Aws3Services>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IViewRenderService, RenderViewToString>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Account/Login";
    // option.LogoutPath = "/Account/Logout";
    option.ExpireTimeSpan = TimeSpan.FromDays(7);
    option.Cookie.Name = "authentication";
    option.AccessDeniedPath = "/Account/Login";
    option.Events = new CookieAuthenticationEvents
    {
        OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddDbContext<FileUploaderContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("FileUploaderContext"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using Microsoft.EntityFrameworkCore;
using RMG.BLL;
using RMG.DAL;
using RMG.DAL.Repository;
using RMG.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using RMG.Utility;
using RMG.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<GenreBll>();
builder.Services.AddScoped<PlatformBll>();
builder.Services.AddScoped<GameBll>();
builder.Services.AddScoped<SubscriptionBll>();
builder.Services.AddScoped<RentalBll>();
builder.Services.AddScoped<ApplicationUserBll>();
builder.Services.AddScoped<SubscriptionHistoryBll>();
builder.Services.AddScoped<ReviewBll>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

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
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{area=Customer}/{controller=Games}/{action=Index}/{id?}");

app.Run();

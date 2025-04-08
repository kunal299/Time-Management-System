using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using UCITMS.Data;
using UCITMS.Data.IRepositories;
using UCITMS.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MySampleMVCWeb.Session"; // Name of the session cookie
    options.Cookie.HttpOnly = true; // Cookie is not accessible via JavaScript
    options.Cookie.IsEssential = true; // Essential cookie for the application
    options.Cookie.Path = "/"; // Cookie is valid for the entire site
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Always use secure cookies
});

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
       .AddMicrosoftIdentityWebApp(builder.Configuration, "EntraId");

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

#region Map Services

// Register repositories

builder.Services.AddScoped<IEngagementRepository, EngagementRepository>(provider =>
    new EngagementRepository(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<IMenuRepository>(provider =>
    new MenuRepository(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>(provider =>
    new TimesheetRepository(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<IHRAdminRepository>(provider =>
    new HRAdminRepository(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<ITaskRepository, TaskRepository>(provider =>
    new TaskRepository(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<IUserInfoRepository>(provider => new UserInfoRepository(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<IUserRepository>(provider =>
    new UserRepository(builder.Configuration.GetConnectionString("connectionString")));


#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// Redirect from root URL to the login page
//app.MapGet("/", () => Results.Redirect("/Routing/Login"));

// Define the main route for your controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Routing}/{action=LandingPage}");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    c.RoutePrefix = "swagger"; // Swagger UI at /swagger
});

app.Run();

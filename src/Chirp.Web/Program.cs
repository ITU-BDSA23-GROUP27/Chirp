using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Data;
using Chirp.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string path = Path.Join(Path.GetTempPath(), "Chirp.db");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Chirp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
    {
        //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GitHub";
    })
    .AddCookie(o =>
    {
        // set the path for the authentication challenge
        o.LoginPath = "/signin";
        // set the path for the sign out
        o.LogoutPath = "/signout";
    })
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication:github:clientId"];
        o.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
        o.CallbackPath = "/signin-github";
    });



builder.Services.AddRazorPages();
builder.Services.AddDbContext<ChirpContext>(options => options.UseSqlite($"Data Source={path}"));
builder.Services.AddScoped<ICheepRepository, Chirp.Infrastructure.CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

var app = builder.Build();

// Database gets seeded at application start
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChirpContext>();
    context.Database.Migrate();
    DbInitializer.SeedDatabase(context);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.None
});

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.Run();
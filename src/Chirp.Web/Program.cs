using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Data;
using Chirp.Infrastructure.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string path = Path.Join(Path.GetTempPath(), "Chirp.db");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ChirpContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ChirpContext>();

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
        o.ClientId = builder.Configuration["authentication_github_clientId"] ?? throw new InvalidOperationException("GitHub Client ID not found.");
        o.ClientSecret = builder.Configuration["authentication_github_clientSecret"] ?? throw new InvalidOperationException("GitHub Client Secret not found.");
        o.CallbackPath = "/signin-github";
    });

builder.Services.AddRazorPages()
        .AddRazorPagesOptions(options =>
        {
            options.Conventions.AuthorizePage("/AboutMe");
            options.Conventions.AuthorizePage("/SeedDb");
        });
        
builder.Services.AddScoped<ICheepRepository, Chirp.Infrastructure.CheepRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();

builder.Services.AddScoped<IValidator<CheepDto>, CheepValidator>();
builder.Services.AddScoped<IValidator<CommentDto>, CommentValidator>();

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
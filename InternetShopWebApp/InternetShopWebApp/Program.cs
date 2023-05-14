using InternetShopWebApp.Context;
using InternetShopWebApp.Data;
using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using InternetShopWebApp.Services;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using NLog.Web;
using NLog;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();

        });
    });

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.

    builder.Services.AddScoped(typeof(UnitOfWork));
    builder.Services.AddScoped(typeof(ProductService));
    builder.Services.AddScoped(typeof(OrderServices));
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<InternetShopContext>();
    builder.Services.AddDbContext<InternetShopContext>();
    builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler =
    ReferenceHandler.IgnoreCycles);

    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.Name = "ShoppingApp";
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
        options.LogoutPath = "/";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        //  401      
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });


    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var shopInternetContext =
        scope.ServiceProvider.GetRequiredService<InternetShopContext>();
        await ShopContextSeed.SeedAsync(shopInternetContext);

        await IdentitySeed.CreateUserRoles(scope.ServiceProvider);
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
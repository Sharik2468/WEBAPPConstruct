using InternetShopWebApp.Context;
using InternetShopWebApp.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

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


// Add services to the container.

//builder.Services.AddDbContext<Context>(opt =>
//opt.UseInMemoryDatabase("Shop"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<ProductContext>(opt =>
//opt.UseModel(ProductModel));
builder.Services.AddDbContext<ShopContext>();
builder.Services.AddControllers().AddJsonOptions(x =>
x.JsonSerializerOptions.ReferenceHandler =
ReferenceHandler.IgnoreCycles);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var shopInternetContext =
    scope.ServiceProvider.GetRequiredService<ShopContext>();
    await ShopContextSeed.SeedAsync(shopInternetContext);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

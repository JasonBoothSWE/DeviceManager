using Microsoft.EntityFrameworkCore;
using DeviceManager.Models;
using MVC5_mockup.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DeviceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DeviceContext") ?? throw new InvalidOperationException("Connection string 'DeviceContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DeviceContext>( opt =>
    opt.UseInMemoryDatabase("DeviceManager"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DeviceApi", Version = "v1" });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add seed initializer for Db
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeviceApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

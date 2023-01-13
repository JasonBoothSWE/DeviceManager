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
    //c.SwaggerDoc("v1", new() { Title = "DeviceApi", Version = "v1" });
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "DeviceApi",
        Version = "v1",
        Description = "Api tool to manage live operating devices",      
    });
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
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeviceApi v1");
        var sidebar = Path.Combine(builder.Environment.ContentRootPath, "wwwroot/custom-sidebar.html");
        c.HeadContent = File.ReadAllText(sidebar);
        c.InjectStylesheet("/css/swagger-custom.css");
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

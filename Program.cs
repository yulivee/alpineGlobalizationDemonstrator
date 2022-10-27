using alpinetest.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<WeatherDbContext>(x => x.UseSqlServer(connectionString));

{
    var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
    optionsBuilder.UseSqlServer(connectionString);
    var context = new WeatherDbContext(optionsBuilder.Options);
    context.Database.Migrate();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

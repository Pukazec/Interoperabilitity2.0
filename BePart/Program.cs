
using BePart;
using BePart.Data;
using BePart.Data_Service;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddSingleton<IZeebeService, ZeebeeService>();

builder.Services.AddCustomEnv("camondo.env");
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CatDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var zeebeService = app.Services.GetService<IZeebeService>();
zeebeService.Deploy("test-process.bpmn");
zeebeService.StartWorkers();

app.Run();

using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
var isProd = builder.Environment.IsProduction();

if (isProd)
{
    Console.WriteLine("--> Using Sql Server");
    builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn")));
}
else
{
    Console.WriteLine("--> Using InMem db");
    builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}

//Add services to the container.
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, isProd);

app.Run();
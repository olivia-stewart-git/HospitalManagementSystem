
using HMS.Data;
using HMS.Service;
using HSM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = new HostApplicationBuilder();
builder.Services.RegisterServices()
				.AddDbContext<HMSDbContext>();

using var host = builder.Build();

var seeder = host.Services.GetService<ISeeder>();
seeder?.SeedDb();

var logonService = host.Services.GetService<ILogonService>();

logonService?.StartLogonProcess();
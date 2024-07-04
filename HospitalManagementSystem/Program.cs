
using HMS.Data;
using HMS.Service;
using HSM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = new HostApplicationBuilder();
builder.Services.RegisterServices()
				.AddDbContext<HMSDbContext>();

using var host = builder.Build();

var seeder = host.Services.GetService<ISeeder>() 
	?? throw new InvalidOperationException("Error in retrieving seeding service");
seeder.SeedDb();

var logonService = host.Services.GetService<ILogonService>()
	?? throw new InvalidOperationException("Error in retrieving seeding service");

logonService.Logon();
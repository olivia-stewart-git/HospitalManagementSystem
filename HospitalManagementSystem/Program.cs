
using System.Text;
using HMS.Data;
using HSM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Loading.....");
HostApplicationBuilder builder = new HostApplicationBuilder();
builder.ConfigureConsole()
	   .Services.RegisterServices()
				.AddDbContext<HMSDbContext>();

using var host = builder.Build();
await new ApplicationEntryPoint().Run(host);

using HMS.Data;
using HMS.Service;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using HSM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Loading.....");
HostApplicationBuilder builder = new HostApplicationBuilder();
builder.ConfigureConsole()
	   .Services.RegisterServices()
				.AddDbContext<HMSDbContext>();

using var host = builder.Build();

var seeder = host.Services.GetService<ISeeder>();
seeder?.SeedDb();

var viewService = host.Services.GetService<IViewService>();
viewService?.SwitchView<LoginView>();
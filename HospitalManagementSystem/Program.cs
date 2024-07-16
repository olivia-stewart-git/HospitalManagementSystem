
using HMS.Data;
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

var cts = new CancellationTokenSource();
_ = new HospitalManagementSystem(cts);

var viewService = host.Services.GetService<IViewService>();
viewService?.SwitchView<LoginView>();

Console.CursorVisible = false;
Console.CancelKeyPress += (sender, args) => cts.Cancel();
while (!cts.IsCancellationRequested)
{
	await Task.Delay(50, cts.Token);
}
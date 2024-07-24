using HMS.Data;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class ApplicationEntryPoint
{
	public async Task Run(IHost host)
	{
		var seeder = host.Services.GetService<ISeeder>();
		seeder?.SeedDb();

		var cts = new CancellationTokenSource();
		_ = new HospitalManagementSystem(cts);

		//Start in our first view
		var viewService = host.Services.GetService<IViewService>();
		viewService?.SwitchView<LoginView>();


		//Keep alive so that the app keeps running even if main thread reaches end of logic
		Console.CursorVisible = false;
		Console.CancelKeyPress += (_, _) => cts.Cancel();
		while (!cts.IsCancellationRequested)
		{
			await Task.Delay(50, cts.Token);
		}
	}
}
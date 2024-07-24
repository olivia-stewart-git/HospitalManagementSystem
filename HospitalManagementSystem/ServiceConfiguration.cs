using HMS.Data;
using HMS.Data.DataAccess;
using HMS.Service;
using HMS.Service.Interaction;
using HMS.Service.ViewService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Environment = HMS.Service.Environment;

namespace HSM;

public static class ServiceConfiguration
{
	/// <summary>
	/// Extension method to bundle registration of DI context.
	/// </summary>
	/// <param name="serviceCollection"></param>
	/// <returns></returns>
	public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton(sp => sp);
		serviceCollection.AddSingleton<IEnvironment, Environment>();
		serviceCollection.AddSingleton<IViewService, ViewService>();
		serviceCollection.AddSingleton<IInputService, InputService>();

		serviceCollection.AddTransient<ILogonService, LogonService>();
		serviceCollection.AddTransient<ISeeder, Seeder>();
		serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
		serviceCollection.AddTransient<IViewWriter, ViewWriter>();
		serviceCollection.AddTransient<IMailService, MailService>();
		return serviceCollection;
	}

	/// <summary>
	/// Configuration for console.
	/// </summary>
	/// <param name="applicationBuilder"></param>
	/// <returns></returns>
	public static HostApplicationBuilder ConfigureConsole(this HostApplicationBuilder applicationBuilder)
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		return applicationBuilder;
	}
}
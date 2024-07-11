using HMS.Data;
using HMS.Data.DataAccess;
using HMS.Service;
using HMS.Service.Interaction;
using HMS.Service.ViewService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Environment = HMS.Service.Environment;

namespace HSM
{
    public static class ServiceConfiguration
    {
	    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	    {
		    serviceCollection.AddSingleton(sp => sp);
		    serviceCollection.AddSingleton<IEnvironment, Environment>();
            serviceCollection.AddSingleton<IViewService, ViewService>();

		    serviceCollection.AddTransient<ILogonService, LogonService>();
            serviceCollection.AddTransient<ISeeder, Seeder>();
            serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
		    serviceCollection.AddTransient<IInputService, InputService>();
		    serviceCollection.AddTransient<IViewWriter, ViewWriter>();
		    return serviceCollection;
	    }

	    public static HostApplicationBuilder ConfigureConsole(this HostApplicationBuilder applicationBuilder)
	    {
		    Console.OutputEncoding = System.Text.Encoding.UTF8;
		    return applicationBuilder;
	    }
    }
}

using HMS.Data;
using HMS.Data.DataAccess;
using HMS.Service;
using Microsoft.Extensions.DependencyInjection;

namespace HSM
{
    public static class ServiceConfiguration
    {
	    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
	    {
		    serviceCollection.AddTransient<ISeeder, Seeder>();
            serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
		    serviceCollection.AddTransient<ILogonService, LogonService>();
		    return serviceCollection;
	    }
    }
}

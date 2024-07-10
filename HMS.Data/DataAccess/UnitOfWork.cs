namespace HMS.Data.DataAccess;

public class UnitOfWork : IUnitOfWork
{
	readonly HMSDbContext dbContext;
	EventHandler commited;

	public UnitOfWork(HMSDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	readonly Dictionary<Type, object> repositoryMap = [];

	public IRepository<T> GetRepository<T>() where T : class
	{
		if (repositoryMap.TryGetValue(typeof(T), out var repository))
		{
			return (IRepository<T>)repository;
		}

		var repositoryInstance = new Repository<T>(dbContext);
		commited += (_, _) => repositoryInstance.DoChange();
		repositoryMap[typeof(T)] = repositoryInstance;
		return repositoryInstance;
	}

	public void Commit()
	{
		dbContext.SaveChanges();
		commited?.Invoke(this, EventArgs.Empty);
	}
}
namespace HMS.Data.DataAccess;

public class UnitOfWork : IUnitOfWork
{
	readonly HMSDbContext dbContext;

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
		repositoryMap[typeof(T)] = repositoryInstance;
		return repositoryInstance;
	}

	public void Commit()
	{
		dbContext.SaveChanges();
	}

	#region DisposePattern

	bool disposed = false;

    protected virtual void Dispose(bool disposing)
	{
		if (!this.disposed)
		{
			if (disposing)
			{
				dbContext.Dispose();
			}
		}
		this.disposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	#endregion
}
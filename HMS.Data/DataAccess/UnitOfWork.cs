namespace HMS.Data.DataAccess;

/// <summary>
/// A single unitOfWork or transaction in the db.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
	readonly HMSDbContext dbContext;
	EventHandler commited;

	public UnitOfWork(HMSDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	readonly Dictionary<Type, object> repositoryMap = [];

	//Generic implementation of repository pattern
	public IRepository<T> GetRepository<T>() where T : class, IDbModel
	{
		if (repositoryMap.TryGetValue(typeof(T), out var repository))
		{
			return (IRepository<T>)repository;
		}

		var repositoryInstance = new Repository<T>(dbContext);
		commited += (_, _) => repositoryInstance.RegisterChanged();
		repositoryMap[typeof(T)] = repositoryInstance;
		return repositoryInstance;
	}

	//Save changes in db. Provides commited event callback
	public void Commit()
	{
		dbContext.SaveChanges();
		commited?.Invoke(this, EventArgs.Empty);
	}
}
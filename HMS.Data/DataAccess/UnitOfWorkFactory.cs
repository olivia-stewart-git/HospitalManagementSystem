namespace HMS.Data.DataAccess;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
	readonly HMSDbContext dbContext;

	public UnitOfWorkFactory(HMSDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public IUnitOfWork CreateUnitOfWork()
	{
		return new UnitOfWork(dbContext);
	}
}
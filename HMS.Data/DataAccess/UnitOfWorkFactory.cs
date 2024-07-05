namespace HMS.Data.DataAccess;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
	readonly HMSDbContext dbContext;

	public UnitOfWorkFactory(HMSDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public UnitOfWork CreateUnitOfWork()
	{
		return new UnitOfWork(dbContext);
	}
}
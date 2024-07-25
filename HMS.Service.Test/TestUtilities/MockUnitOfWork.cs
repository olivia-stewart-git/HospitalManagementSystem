using HMS.Data;
using HMS.Data.DataAccess;

namespace HMS.Service.TestUtilities;

public class MockUnitOfWork : IUnitOfWork
{
	readonly object[] mockRepositories;

	public MockUnitOfWork(params object[] mockRepositories)
	{
		this.mockRepositories = mockRepositories;
	}

	public IRepository<T> GetRepository<T>() where T : class, IDbModel
	{
		return mockRepositories.OfType<IRepository<T>>().FirstOrDefault()
			?? throw new InvalidOperationException("Not setup mock repositories");
	}

	public void Commit()
	{
	}
}
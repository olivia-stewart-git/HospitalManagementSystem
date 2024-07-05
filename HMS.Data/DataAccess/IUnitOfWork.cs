namespace HMS.Data.DataAccess;

public interface IUnitOfWork : IDisposable
{
	IRepository<T> GetRepository<T>() where T : class;
	void Commit();
}
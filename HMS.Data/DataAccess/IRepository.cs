using HMS.Common;
using System.Linq.Expressions;

namespace HMS.Data;

public interface IRepository<T> : IChangePropagator<IEnumerable<T>> where T : class
{
	T? GetById(Guid id);
	IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate, string[]? includedProperties);

	IEnumerable<T> Get();
    IEnumerable<T> Get(string[]? includedProperties);
	bool Exists(Expression<Func<T, bool>> predicate);
	void InsertRange(IEnumerable<T> entities);
	void InsertRange(params T[] entities);
	void Insert(T entity);
	void Delete(Guid id);
	void Delete(T entityToDelete);
	void Update(T entityToUpdate);
}
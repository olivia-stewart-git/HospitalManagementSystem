using System.Linq.Expressions;
using HMS.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace HMS.Data;

public interface IRepository<T> : IChangePropagator<IEnumerable<T>> where T : class
{
	T? GetById(Guid id);
	IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate, int rowCount = -1);
	IEnumerable<T> Get(int rowCount = -1);
	bool Exists(Expression<Func<T, bool>> predicate);
	void InsertRange(IEnumerable<T> entities);
	void InsertRange(params T[] entities);
	void Insert(T entity);
	void Delete(Guid id);
	void Delete(T entityToDelete);
	void Update(T entityToUpdate);
}
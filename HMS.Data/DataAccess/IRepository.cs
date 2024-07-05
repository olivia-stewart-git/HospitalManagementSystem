using System.Linq.Expressions;

namespace HMS.Data;

public interface IRepository<T> where T : class
{
	T? GetById(Guid id);
	IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate, int rowCount = -1);
	void Insert(T entity);
	void Delete(Guid id);
	void Delete(T entityToDelete);
	void Update(T entityToUpdate);
}
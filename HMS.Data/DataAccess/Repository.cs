using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HMS.Data.DataAccess;

public class Repository<T> : IRepository<T> where T : class
{
	readonly HMSDbContext dbContext;
	readonly DbSet<T> entitySet;

	public Repository(HMSDbContext dbContext)
	{
		this.dbContext = dbContext;
		this.entitySet = dbContext.Set<T>();
	}

	public T? GetById(Guid id)
	{
		ArgumentNullException.ThrowIfNull(id);
		return entitySet.Find(id);
	}

	public IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate, int rowCount = -1)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		IQueryable<T> queryable = entitySet;
		queryable = queryable.Where(predicate);

		if (rowCount > 0)
		{
			queryable = queryable.Take(rowCount);
		}

		return queryable.ToList();
	}

	public void Insert(T entity)
	{
		dbContext.Add(entity);
	}

	public void Delete(Guid id)
	{
		var entityToDelete = entitySet.Find(id);
		if (entityToDelete != null)
		{
			entitySet.Remove(entityToDelete);
		}
	}

	public void Delete(T entityToDelete)
	{
		if (entitySet.Entry(entityToDelete).State == EntityState.Detached)
		{
			entitySet.Attach(entityToDelete);
		}
		entitySet.Remove(entityToDelete);
	}

	public void Update(T entityToUpdate)
	{
		entitySet.Attach(entityToUpdate);
		dbContext.Entry(entityToUpdate).State = EntityState.Modified;
	}
}
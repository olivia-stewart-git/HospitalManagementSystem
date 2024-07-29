using HMS.Data;
using HMS.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HMS.Service.TestUtilities;

public static class DbContextMockExtensions
{
	public static Mock<DbSet<T>> SetupDataRepository<T>(this Mock<HMSDbContext> mockContext, IEnumerable<T> dataRepository) where T : class, IDbModel
	{
		var data = dataRepository.AsQueryable();
		var mockSet = new Mock<DbSet<T>>();
		mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
		mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
		mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
		mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
		mockContext.Setup(x => x.Set<T>()).Returns(mockSet.Object);
		return mockSet;
	}
}
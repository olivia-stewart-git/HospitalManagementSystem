using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.TestUtilities;
using Moq;

namespace HMS.Data.Test;

public class RepositoryTest
{
	[Test]
	public void TestReturnsById()
	{
		//Arrange
		var expectedModel = new DoctorModel() { User = new UserModel() };
		var set = new[]
		{
			expectedModel,
			new (),
			new (),
			new (),
			new (),
		};
		var mockDbContext = new Mock<HMSDbContext>();
		var mockSet = mockDbContext.SetupDataRepository(set);
		mockSet.Setup(x => x.Find(expectedModel.DCT_PK)).Returns(expectedModel);

		var repository = new Repository<DoctorModel>(mockDbContext.Object);

		//Act
		var result = repository.GetById(expectedModel.DCT_PK);
		Assert.IsNotNull(result);
		Assert.That(result, Is.EqualTo(expectedModel));
	}

	[Test]
	public void TestGetWhereReturnsExpectedValues()
	{
		//Arrange
		var expectedModel = new UserModel() { USR_FirstName = "Tim" };
		var set = new[]
		{
			expectedModel,
			new (),
			new (),
			new (),
			new (),
		};
		var mockDbContext = new Mock<HMSDbContext>();
		mockDbContext.SetupDataRepository(set);

		var repository = new Repository<UserModel>(mockDbContext.Object);

		//Act
		var result = repository.GetWhere(x => x.USR_FirstName == "Tim").FirstOrDefault();
		Assert.IsNotNull(result);
		Assert.That(result, Is.EqualTo(expectedModel));
	}

	[TestCase("Brett", false)]
	[TestCase("Tim", true)]
	public void TestExists(string condition, bool expected)
	{
		//Arrange
		var set = new[]
		{
			new UserModel() { USR_FirstName = "Tim" },
			new (),
			new (),
			new (),
			new (),
		};
		var mockDbContext = new Mock<HMSDbContext>();
		mockDbContext.SetupDataRepository(set);

		var repository = new Repository<UserModel>(mockDbContext.Object);

		//Act
		var result = repository.Exists(x => x.USR_FirstName == condition);
		Assert.That(result, Is.EqualTo(expected));
	}

	[Test]
	public void TestGetReturnsExpected()
	{
		var set = new UserModel[]
		{
			new (),
			new (),
			new (),
			new (),
			new (),
			new (),
			new (),
		};
		var mockDbContext = new Mock<HMSDbContext>();
		mockDbContext.SetupDataRepository(set);

		var repository = new Repository<UserModel>(mockDbContext.Object);

		//Act
		var result = repository.Get();
		Assert.IsNotNull(result);
		Assert.That(result, Is.EquivalentTo(set));
	}
}
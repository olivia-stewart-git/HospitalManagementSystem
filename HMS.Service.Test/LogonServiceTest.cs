using HMS.Data;
using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.Interaction;
using HMS.Service.ViewService;
using Moq;
using System.Linq.Expressions;
using HMS.Service.TestUtilities;
using HMS.Service.ViewService.AppViews;

namespace HMS.Service.Test;

internal class LogonServiceTest
{
	[TestCase(123141, "test", true)]
	[TestCase(123141, "nope", false)]
	[TestCase(28515, "ad213", false)]
    public void TestLogon(int userId, string password, bool expectedLogon)
	{
		//Arrange
		var unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
		var mockUser = new UserModel()
		{
			USR_PK = Guid.NewGuid(),
			USR_Password = "test",
			USR_ID = 123141,
		};
		var mockRepository = new Mock<IRepository<UserModel>>();
		var mockUnitOfWork = new Mock<IUnitOfWork>();

		mockRepository.Setup(x => x.GetWhere(It.IsAny<Expression<Func<UserModel, bool>>>(), It.IsAny<int>())).Returns([mockUser]);
		unitOfWorkFactory.Setup(x => x.CreateUnitOfWork()).Returns(mockUnitOfWork.Object);

		mockUnitOfWork.Setup(x => x.GetRepository<UserModel>()).Returns(mockRepository.Object);

		var mockInputService = new Mock<IInputService>();
		mockInputService.Setup(x => x.ReadInput()).Returns(password);
		mockInputService.Setup(x => x.ReadIntegerInput()).Returns(userId);

		var mockViewService = new Mock<IViewService>();
		mockViewService.Setup(x => x.CurrentView).Returns(new LoginView(Mock.Of<ILogonService>()));

		var logonService = new LogonService(mockInputService.Object, mockViewService.Object, unitOfWorkFactory.Object) { DoRepeatLogon = false };

		//Act
		logonService.StartLogonProcess();

		//Assert
		Assert.That(logonService.IsLoggedIn, Is.EqualTo(expectedLogon));
	}
}
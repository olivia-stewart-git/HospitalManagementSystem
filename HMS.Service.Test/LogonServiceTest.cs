using HMS.Data;
using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.Interaction;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using Moq;
using System.Linq.Expressions;

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
		SetupRepositoryMocksForType(mockUnitOfWork, typeof(AdministratorModel));

        var mockInputService = new Mock<IInputService>();

		var mockViewService = new Mock<IViewService>();
		mockViewService.Setup(x => x.CurrentView).Returns(new LoginView(Mock.Of<ILogonService>()));

		var logonService = new LogonService(mockInputService.Object, mockViewService.Object, unitOfWorkFactory.Object, Mock.Of<IEnvironment>()) { DoRepeatLogon = false };

		//Act
		logonService.ExecuteLogin(userId, password, null);

		//Assert
		Assert.That(logonService.IsLoggedIn, Is.EqualTo(expectedLogon));
	}

	[TestCase(SystemRole.Doctor, typeof(DoctorModel))]
	[TestCase(SystemRole.Patient, typeof(PatientModel))]
	[TestCase(SystemRole.Admin, typeof(AdministratorModel))]
    public void TestLoadsCorrectMenu(SystemRole expectedRole, Type objectType)
	{
		//Arrange
		var unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
		var mockUser = new UserModel()
		{
			USR_PK = Guid.NewGuid(),
			USR_Password = "test",
			USR_ID = 1,
		};
		var mockRepository = new Mock<IRepository<UserModel>>();
		var mockUnitOfWork = new Mock<IUnitOfWork>();

		mockRepository.Setup(x => x.GetWhere(It.IsAny<Expression<Func<UserModel, bool>>>(), It.IsAny<int>())).Returns([mockUser]);
		unitOfWorkFactory.Setup(x => x.CreateUnitOfWork()).Returns(mockUnitOfWork.Object);

		mockUnitOfWork.Setup(x => x.GetRepository<UserModel>()).Returns(mockRepository.Object);

        SetupRepositoryMocksForType(mockUnitOfWork, objectType);

        var mockInputService = new Mock<IInputService>();

		var mockViewService = new Mock<IViewService>();
		mockViewService.Setup(x => x.CurrentView).Returns(new LoginView(Mock.Of<ILogonService>()));

		var env = new Environment();
		var logonService = new LogonService(mockInputService.Object, mockViewService.Object, unitOfWorkFactory.Object, env) { DoRepeatLogon = false };

		//Act
		logonService.ExecuteLogin(1, "test", null);

		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(logonService.IsLoggedIn, Is.EqualTo(true));
			Assert.That(env.CurrentUser, Is.EqualTo(mockUser.USR_PK));
			Assert.That(env.CurrentRole, Is.EqualTo(expectedRole));
		});
	}

	void SetupRepositoryMocksForType(Mock<IUnitOfWork> mockUnitOfWork, Type objectType)
	{
		var mockDoctorRepository = new Mock<IRepository<DoctorModel>>();
		mockDoctorRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<DoctorModel, bool>>>()))
			.Returns(objectType == typeof(DoctorModel));
		mockUnitOfWork.Setup(x => x.GetRepository<DoctorModel>()).Returns(mockDoctorRepository.Object);

		var mockPatientRepository = new Mock<IRepository<PatientModel>>();
		mockPatientRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<PatientModel, bool>>>()))
			.Returns(objectType == typeof(PatientModel));
		mockUnitOfWork.Setup(x => x.GetRepository<PatientModel>()).Returns(mockPatientRepository.Object);

		var mockAdminRepository = new Mock<IRepository<AdministratorModel>>();
		mockAdminRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<AdministratorModel, bool>>>()))
			.Returns(objectType == typeof(AdministratorModel));
		mockUnitOfWork.Setup(x => x.GetRepository<AdministratorModel>()).Returns(mockAdminRepository.Object);
    }
}
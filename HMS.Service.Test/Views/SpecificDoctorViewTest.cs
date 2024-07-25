using System.Linq.Expressions;
using HMS.Data;
using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.TestUtilities;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using HMS.Service.ViewService.Controls;
using Moq;

namespace HMS.Service.Views.Test;

internal class SpecificDoctorViewTest
{
	SpecificDoctorView specificDoctorView;
	Mock<IRepository<DoctorModel>> mockRepository;

    [SetUp]
	public void Setup()
	{
		var mockEnvironment = new Mock<IEnvironment>();
		var user = new UserModel() { USR_FirstName = "Test", USR_LastName = "LastName" };
		mockEnvironment.Setup(x => x.CurrentUser).Returns(user);

		mockRepository = new Mock<IRepository<DoctorModel>>();

		var unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
		unitOfWorkFactory.Setup(x => x.CreateUnitOfWork())
			.Returns(new MockUnitOfWork(mockRepository.Object));

		specificDoctorView = new SpecificDoctorView(unitOfWorkFactory.Object, Mock.Of<IViewService>());

		var builder = new ViewBuilder(specificDoctorView);
		specificDoctorView.BuildView(builder);
		builder.Build();
	}

	[Test]
	public void TestErrorIfNoIdEntered()
	{
		var inputBox = specificDoctorView.Q<InputField>("Doctor-id-input");
		Assert.NotNull(inputBox);
		inputBox.NavigateEnter();
		inputBox.OnEnterInput();

		var outputBox = specificDoctorView.Q<OutputBox>("Doctor-output");
		Assert.NotNull(outputBox);
		Assert.That(outputBox.Enabled, Is.True);
		Assert.That(outputBox.State, Is.EqualTo(OutputBox.OutputState.Error));
    }

	[Test]
	public void TestErrorIfDoctor()
	{
		mockRepository.Setup(x => x.GetWhere(It.IsAny<Expression<Func<DoctorModel, bool>>>(), It.IsAny<string[]>()))
			.Returns(Array.Empty<DoctorModel>());

        var inputBox = specificDoctorView.Q<InputField>("Doctor-id-input");
		Assert.NotNull(inputBox);
		inputBox.Contents = "1";

		inputBox.NavigateEnter();
		inputBox.OnEnterInput();

		var outputBox = specificDoctorView.Q<OutputBox>("Doctor-output");
		Assert.NotNull(outputBox);
		Assert.That(outputBox.Enabled, Is.True);
		Assert.That(outputBox.State, Is.EqualTo(OutputBox.OutputState.Warn));
	}

	[Test]
	public void TestWhenCorrectId()
	{
		var testDoctor = new DoctorModel() { DCT_User = new UserModel() { USR_ID = 12314 } };

        mockRepository.Setup(x => x.GetWhere(It.IsAny<Expression<Func<DoctorModel, bool>>>(), It.IsAny<string[]>()))
			.Returns([testDoctor]);

		var inputBox = specificDoctorView.Q<InputField>("Doctor-id-input");
		Assert.NotNull(inputBox);
		inputBox.Contents = "12314";

		inputBox.NavigateEnter();
		inputBox.OnEnterInput();

		var outputBox = specificDoctorView.Q<OutputBox>("Doctor-output");
		Assert.NotNull(outputBox);
		Assert.That(outputBox.Enabled, Is.False);

		var tableView = specificDoctorView.Q<TableView<DoctorModel>>("Doctors");
		Assert.NotNull(tableView);
		tableView.Rows.Contains(testDoctor);
	}
}
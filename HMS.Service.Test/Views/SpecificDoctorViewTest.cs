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

internal class ListAllPatientsViewTest
{
	ListAllPatientsView allPatientsView;
	Mock<IRepository<PatientModel>> mockRepository;

	[SetUp]
	public void Setup()
	{
		mockRepository = new Mock<IRepository<PatientModel>>();

		var unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
		unitOfWorkFactory.Setup(x => x.CreateUnitOfWork())
			.Returns(new MockUnitOfWork(mockRepository.Object));

		allPatientsView = new ListAllPatientsView(Mock.Of<IViewService>(), unitOfWorkFactory.Object);

		var builder = new ViewBuilder(allPatientsView);
		allPatientsView.BuildView(builder);
		builder.Build();
	}

	[Test]
	public void TestErrorShownWhenNoPatients()
	{
		mockRepository.Setup(x => x.Get(It.IsAny<string[]>()))
			.Returns(Array.Empty<PatientModel>());

		allPatientsView.OnBecomeActive();

		var outputBox = allPatientsView.Q<OutputBox>("patient-details-outputBox");
		Assert.NotNull(outputBox);
		Assert.That(outputBox.Enabled, Is.True);
		Assert.That(outputBox.State, Is.EqualTo(OutputBox.OutputState.Error));
    }

	[Test]
	public void TestGetsAllPatients()
	{
		List<PatientModel> patientCollection =
		[
			new PatientModel() { PAT_User = new UserModel() { USR_FirstName = "testUser" } },
			new PatientModel() { PAT_User = new UserModel() { USR_FirstName = "testUser1" } },
			new PatientModel() { PAT_User = new UserModel() { USR_FirstName = "testUser2" } },
			new PatientModel() { PAT_User = new UserModel() { USR_FirstName = "testUser3" } },
		];

        mockRepository.Setup(x => x.Get(It.IsAny<string[]>()))
			.Returns(patientCollection);

		allPatientsView.OnBecomeActive();

		var outputBox = allPatientsView.Q<OutputBox>("patient-details-outputBox");
		Assert.NotNull(outputBox);
		Assert.That(outputBox.Enabled, Is.False);

		var table = allPatientsView.Q<TableView<PatientModel>>("Patients");
		Assert.That(table.Rows, Is.EqualTo(patientCollection));
	}
}


internal class SpecificDoctorViewTest
{
	SpecificDoctorView specificDoctorView;
	Mock<IRepository<DoctorModel>> mockRepository;

    [SetUp]
	public void Setup()
	{
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
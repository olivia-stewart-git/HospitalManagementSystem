using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class PatientDoctorMenuView : View
{
	readonly IEnvironment environment;
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;

	public PatientDoctorMenuView(IEnvironment environment, IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.environment = environment;
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "My Doctor"))
			.AddControl(new OutputBox(string.Empty, "doctor-details-outputBox") { Enabled = false })
			.AddControl(new TableView<DoctorModel>("Doctor Details",
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_Email, overrideName: "Email"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<DoctorModel>(x => $"{x.DCT_User.USR_Address_State}, {x.DCT_User.USR_Address_Postcode}, {x.DCT_User.USR_Address_Line1} {x.DCT_User.USR_Address_Line2}", overrideName: "Address")
				));
	}

	public override void OnBecomeActive()
	{
		ArgumentNullException.ThrowIfNull(environment.CurrentUser);
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();

		var outputBox = Q<OutputBox>("doctor-details-outputBox");
		var detailsTable = Q<TableView<DoctorModel>>("Doctor Details");

		var patient = patientRepository.GetWhere(x => x.PAT_USR_ID == environment.CurrentUser.USR_PK,
				rowCount: 1,
				includedProperties:
				[
					nameof(PatientModel.PAT_Doctor),
					nameof(PatientModel.PAT_Doctor) + "." + nameof(DoctorModel.DCT_User)
				])
			.SingleOrDefault();

		if (patient == null)
		{
			detailsTable.Enabled = false;
			outputBox.Enabled = true;
			outputBox.SetState("Error Occurred Retrieving Patient information", OutputBox.OutputState.Error);
		}

		var doctor = patient.PAT_Doctor;

		if (doctor != null)
		{
			detailsTable.Enabled = true;
			detailsTable.Update([doctor]);
		}
		else
		{
			detailsTable.Enabled = false;
			outputBox.Enabled = true;
			outputBox.SetState("No Doctor found for Patient", OutputBox.OutputState.Warn);
		}
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
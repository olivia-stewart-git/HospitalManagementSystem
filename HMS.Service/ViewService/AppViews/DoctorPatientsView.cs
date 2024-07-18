using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class DoctorPatientsView : View
{
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IEnvironment environment;
	readonly IViewService viewService;

	public DoctorPatientsView(IUnitOfWorkFactory unitOfWorkFactory, IEnvironment environment, IViewService viewService)
	{
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.environment = environment;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "My Patients"))
			.AddControl(new OutputBox(string.Empty, "doctor-patients-output") { Enabled = false })
			.AddControl(new TableView<PatientModel>("Patients",
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_Email, overrideName: "Email Address"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<PatientModel>(x => $"{x.PAT_User.USR_Address_State}, {x.PAT_User.USR_Address_Postcode}, {x.PAT_User.USR_Address_Line1} {x.PAT_User.USR_Address_Line2}", overrideName: "Address")
				));
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();

        var doctor = doctorRepository.GetWhere(x => x.DCT_USR_ID == environment.CurrentUser.USR_PK, 
	        includedProperties:
			[
				nameof(DoctorModel.DCT_Patients),
				nameof(DoctorModel.DCT_Patients) + "." + nameof(PatientModel.PAT_User)
            ]).SingleOrDefault();

		var tableView = Q<TableView<PatientModel>>("Patients");

		if (doctor != null)
		{
			tableView.Update(doctor.DCT_Patients);
		}
		else
		{
			tableView.Enabled = false;
			var outputBox = Q<OutputBox>("doctor-patients-output");
			outputBox.SetState("Error occured finding patients", OutputBox.OutputState.Error);
		}
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
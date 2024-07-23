using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class ListAllPatientsView : View
{
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	TableView<PatientModel> tableView;
	OutputBox outputBox;

	public ListAllPatientsView(IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "All Patients on System"))
			.AddControl(new OutputBox(string.Empty, "patient-details-outputBox"))
			.Disabled()
			.Place(ref outputBox)
			.AddControl(new TableView<PatientModel>("Patients",
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_Email, overrideName: "Email Address"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<PatientModel>(x => $"{x.PAT_User.USR_Address_State}, {x.PAT_User.USR_Address_Postcode}, {x.PAT_User.USR_Address_Line1} {x.PAT_User.USR_Address_Line2}", overrideName: "Address")
				))
			.Place(ref tableView);
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<PatientModel>();
		var patients = doctorRepository.Get(includedProperties: [nameof(PatientModel.PAT_User)]).ToList();

		if (patients.Count > 0)
		{
			tableView.Update(patients);
		}
		else
		{
			outputBox.Enabled = true;
			outputBox.SetState("No Patients found in system", OutputBox.OutputState.Warn);
			tableView.Enabled = false;
		}
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
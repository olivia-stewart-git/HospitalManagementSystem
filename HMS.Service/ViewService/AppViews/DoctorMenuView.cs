using HMS.Data.DataAccess;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class DoctorMenuView : View
{
	readonly IEnvironment environment;
	readonly IViewService viewService;

	public DoctorMenuView(IEnvironment environment, IViewService viewService)
	{
		this.environment = environment;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Doctor Menu"))
			.AddControl(new Label($"Welcome to DOTNET Hospital Management System {environment.CurrentUser.USR_FullName}"))
			.AddControl(new NewLine())
			.AddControl(new OptionsList("doctor-menu-options", "Please Choose An Option",
				new SelectionOption("List Doctor Details", OnListDoctorDetails),
				new SelectionOption("List Patients", OnListPatients),
				new SelectionOption("List Appointments", OnListAppointments),
				new SelectionOption("Check Particular Patient", OnCheckParticularPatient),
				new SelectionOption("List Appointments With Patient", OnListAppointmentsWithPatient),
				new SelectionOption("Logout", OnLogout),
				new SelectionOption("Exit", OnExit)));
	}

	public void OnListDoctorDetails(SelectionOption option)
	{
		viewService.SwitchView<DoctorDetailsView>();
	}

	public void OnListPatients(SelectionOption option)
	{
	}

	public void OnListAppointments(SelectionOption option)
	{
		viewService.SwitchView<DoctorAppointmentsView>();
	}

	public void OnCheckParticularPatient(SelectionOption option)
	{
	}

	public void OnListAppointmentsWithPatient(SelectionOption option)
	{
	}
	public void OnLogout(SelectionOption option)
	{
	}

	public void OnExit(SelectionOption option)
	{
		HospitalManagementSystem.QuitApplication();
	}
}
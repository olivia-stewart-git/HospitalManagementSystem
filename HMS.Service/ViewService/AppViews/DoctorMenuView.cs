using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class DoctorMenuView : View
{
	readonly IEnvironment environment;
	readonly IViewService viewService;
	readonly ILogonService logonService;

	public DoctorMenuView(IEnvironment environment, IViewService viewService, ILogonService logonService)
	{
		this.environment = environment;
		this.viewService = viewService;
		this.logonService = logonService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Doctor Menu"))
			.AddControl(new Label($"Welcome to DOTNET Hospital Management System {environment.CurrentUser.USR_FullName}", "main-label"))
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

	void OnListDoctorDetails(SelectionOption option)
	{
		viewService.SwitchView<DoctorDetailsView>();
	}

	void OnListPatients(SelectionOption option)
	{
		viewService.SwitchView<DoctorPatientsView>();
	}

	void OnListAppointments(SelectionOption option)
	{
		viewService.SwitchView<DoctorAppointmentsView>();
	}

	void OnCheckParticularPatient(SelectionOption option)
	{
		viewService.SwitchView<SpecificPatientView>();
	}

	public void OnListAppointmentsWithPatient(SelectionOption option)
	{
		viewService.SwitchView<SpecificPatientAppointmentsView>();
	}

	public void OnLogout(SelectionOption option)
	{
		logonService.Logout();
	}

	public void OnExit(SelectionOption option)
	{
		HospitalManagementSystem.QuitApplication();
	}
}
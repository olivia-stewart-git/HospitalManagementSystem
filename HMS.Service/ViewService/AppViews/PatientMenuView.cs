using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class PatientMenuView : View
{
	readonly IEnvironment environment;
	readonly IViewService viewService;
	readonly ILogonService logonService;

	public PatientMenuView(IEnvironment environment, IViewService viewService, ILogonService logonService)
	{
		this.environment = environment;
		this.viewService = viewService;
		this.logonService = logonService;
	}

    public override void BuildView(ViewBuilder viewBuilder)
    {
	    viewBuilder
		    .AddControl(new PageHeader("DOTNET Hospital Management System", "Patient Menu"))
		    .AddControl(new Label($"Welcome to DOTNET Hospital Management System {environment.CurrentUser.USR_FullName}"))
		    .AddControl(new NewLine())
		    .AddControl(new OptionsList("patient-menu-options", "Please Choose An Option",
			    new SelectionOption("List Patient Details", OnListPatientDetails),
			    new SelectionOption("List my Doctor Details", OnProvideDoctorDetails),
			    new SelectionOption("List All Appointments", OnListPatientAppointments),
			    new SelectionOption("Book Appointment", OnBookAppointment),
			    new SelectionOption("Logout", OnLogout),
			    new SelectionOption("Exit", OnExit)));
    }

    void OnListPatientDetails(SelectionOption option)
    {
		viewService.SwitchView<PatientDetailsView>();
    }

    void OnProvideDoctorDetails(SelectionOption option)
    {
	    viewService.SwitchView<PatientDoctorMenuView>();
    }

    void OnListPatientAppointments(SelectionOption option)
    {
	    viewService.SwitchView<PatientAppointmentsView>();
    }

    void OnLogout(SelectionOption option)
    {
	    logonService.Logout();
    }

    void OnBookAppointment(SelectionOption option)
    {
	    throw new NotImplementedException();
    }

    void OnExit(SelectionOption option)
    {
		HospitalManagementSystem.QuitApplication();
    }
}
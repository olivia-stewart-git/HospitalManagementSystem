using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class AdministratorMenuView : View
{
	readonly IEnvironment environment;
	readonly ILogonService logonService;
	readonly IViewService viewService;

	public AdministratorMenuView(IEnvironment environment, ILogonService logonService, IViewService viewService)
	{
		this.environment = environment;
		this.logonService = logonService;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Administrator Menu"))
			.AddControl(new Label($"Welcome to DOTNET Hospital Management System {environment.CurrentUser.USR_FullName}"))
			.AddControl(new NewLine())
			.AddControl(new OptionsList("administrator-menu-options", "Please Choose An Option",
				new SelectionOption("List All Doctors", OnListAllDoctors),
				new SelectionOption("Check Doctor Details", OnCheckDoctorDetails),
				new SelectionOption("List All Patients", OnListAllPatients),
				new SelectionOption("Check Patient Details", OnCheckPatientDetails),
				new SelectionOption("Add Patient", OnAddPatient),
				new SelectionOption("Add Doctor", OnAddDoctor),
                new SelectionOption("Logout", OnLogout),
				new SelectionOption("Exit", OnExit)));
	}

	void OnAddDoctor(SelectionOption option)
	{
		viewService.SwitchView<AddUserView<DoctorModel>>();
    }

	void OnAddPatient(SelectionOption option)
	{
		viewService.SwitchView<AddUserView<PatientModel>>();
	}

	void OnCheckPatientDetails(SelectionOption option)
	{
		viewService.SwitchView<SpecificPatientView>();
	}

	void OnListAllPatients(SelectionOption option)
	{
		viewService.SwitchView<ListAllPatientsView>();
    }

	void OnCheckDoctorDetails(SelectionOption option)
	{
		viewService.SwitchView<SpecificDoctorView>();
	}

	void OnListAllDoctors(SelectionOption option)
	{
		viewService.SwitchView<ListAllDoctorsView>();
	}

	void OnLogout(SelectionOption option)
	{
		logonService.Logout();
	}

	void OnExit(SelectionOption option)
	{
		HospitalManagementSystem.QuitApplication();
	}
}
using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class ViewSpecificPatientView : View
{
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;

	public ViewSpecificPatientView(IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Check Patient Details"))
			.AddControl(new InputField("Enter the ID of the patient you want to check:", "patient-id-input"))
			.AddControl(new TableView<PatientModel>("Patients",
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_Email, overrideName: "Email Address"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<PatientModel>(x => $"{x.PAT_User.USR_Address_State}, {x.PAT_User.USR_Address_Postcode}, {x.PAT_User.USR_Address_Line1} {x.PAT_User.USR_Address_Line2}", overrideName: "Address")
				) { Enabled = false });
	}

	public override void OnBecomeActive()
	{
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
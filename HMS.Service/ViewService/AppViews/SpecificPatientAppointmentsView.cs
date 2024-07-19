using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class SpecificPatientAppointmentsView : View
{
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;

	public SpecificPatientAppointmentsView(IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	int userID;
	OutputBox outputBox;

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Appointments With"))
			.AddControl(new InputField("Enter the ID of the patient you want to view the appointment for:", "appointment-id-input") { AllowOnlyNumeric = true })
			.Setup<InputField>(x =>
			{
				x.BindProperty<string>(prop => int.TryParse(prop, out userID), nameof(InputField.Contents));
				x.Completed += HandleInput;
			})

			.AddControl(new OutputBox(string.Empty, "output-box"))
			.Place(ref outputBox)

			.AddControl(new TableView<AppointmentModel>("All Appointments",
				new TableViewColumn<AppointmentModel>(x => x.APT_Doctor.DCT_User.USR_FullName, overrideName: "Doctor"),
				new TableViewColumn<AppointmentModel>(x => x.APT_Patient.PAT_User.USR_FullName, overrideName: "Patient"),
				new TableViewColumn<AppointmentModel>(x => x.APT_Description),
				new TableViewColumn<AppointmentModel>(x => x.APT_AppointmentTime, overrideName: "Appointment Time")) { Enabled = false });
	}

	void HandleInput(object? sender, EventArgs e)
	{
	}
}
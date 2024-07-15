using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class DoctorAppointmentsView : View
{
	readonly IEnvironment environment;
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;

	public DoctorAppointmentsView(IEnvironment environment, IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.environment = environment;
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "All Appointments"))
			.AddControl(new OutputBox(string.Empty, "doctor-appointments-outputBox") { Enabled = false })
			.AddControl(new TableView<AppointmentModel>("All Appointments",
				new TableViewColumn<AppointmentModel>(x => x.APT_Doctor.DCT_User.USR_FullName, overrideName: "Doctor"),
				new TableViewColumn<AppointmentModel>(x => x.APT_Patient.PAT_User.USR_FullName, overrideName: "Patient"),
				new TableViewColumn<AppointmentModel>(x => x.APT_Description),
				new TableViewColumn<AppointmentModel>(x => x.APT_AppointmentTime)));
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var appointmentsRepository = unitOfWork.GetRepository<AppointmentModel>();

		var outputBox = Q<OutputBox>("doctor-appointments-outputBox");

		var detailsTable = Q<TableView<AppointmentModel>>("All Appointments");

			var appointments = appointmentsRepository
				.GetWhere(x => x.APT_Doctor.DCT_USR_ID == environment.CurrentUser.USR_PK,
				[
					nameof(AppointmentModel.APT_Doctor),
					nameof(AppointmentModel.APT_Doctor) + "." + nameof(AppointmentModel.APT_Doctor.DCT_User),
					nameof(AppointmentModel.APT_Patient),
					nameof(AppointmentModel.APT_Patient) + "." + nameof(AppointmentModel.APT_Patient.PAT_User)
				])
				.ToList();

		if (appointments.Count > 0)
		{
			detailsTable.Update(appointments);
		}
		else
		{
			detailsTable.Enabled = false;
			outputBox.Enabled = true;
			outputBox.SetState("No appointments for Doctor", OutputBox.OutputState.Warn);
		}
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
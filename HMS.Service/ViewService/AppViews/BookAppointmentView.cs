using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class BookAppointmentView : View
{
	readonly IEnvironment environment;
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	string doctorName = string.Empty;
	string inputDescription = string.Empty;
	DateTime? inputDatetime;

	OutputBox outputBox;
	ControlContainer appointmentSettingsContainer;
	ControlContainer doctorOptionsContainer;
	InteractableTableView<DoctorModel> doctorTable;

	public BookAppointmentView(IEnvironment environment, IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.environment = environment;
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Book Appointment"))
			.AddControl(new OutputBox(string.Empty))
				.Place(ref outputBox)
				.Disabled()

            .StartContainer("doctor-options-container")
			.Place(ref doctorOptionsContainer)
				.AddControl(new InteractableTableView<DoctorModel>("Doctor Details",
					new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name"),
					new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_Email, overrideName: "Email"),
					new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_PhoneNumber, overrideName: "Phone")))
					.Setup<InteractableTableView<DoctorModel>>(x => x.Selected += OnDoctorSelected)
					.Place(ref doctorTable)
			.EndContainer()

			.StartContainer("appointment-container")
			.Place(ref appointmentSettingsContainer)
			.Disabled()
				.AddControl(new Label($"You are booking an appointment with {doctorName}"))
				.AddControl(new InputField("Description of appointment:", "input-description"))
					.Setup<InputField>(x =>
					{
						x.BindProperty<string>(property => inputDescription = property, nameof(InputField.Contents));
					})
				.AddControl(new InputField("Appointment Date:", "input-date"))
					.Setup<InputField>(x =>
					{
						x.BindProperty<string>(property =>
						{
							if (DateTime.TryParse(property, out var dateTime))
							{
								inputDatetime = dateTime;
							}
							else
							{
								inputDatetime = null;
							}
						}, nameof(InputField.Contents));
					})
				.AddControl(new Button("Book Appointment", "book-button"))
					.Setup<Button>(x => x.Interacted += OnBook)
			.EndContainer();
	}

	void OnDoctorSelected(DoctorModel value)
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();

		var patient = patientRepository.GetWhere(x => x.PAT_USR_ID == environment.CurrentUser.USR_PK).FirstOrDefault();
		if (patient == null)
		{
			outputBox.Enabled = true;
			outputBox.SetState("error occured retrieving patient info", OutputBox.OutputState.Error);
		}
		else
		{
			doctorName = value.DCT_User.USR_FullName;
			patient.PAT_Doctor = value;
			patientRepository.Update(patient);
			unitOfWork.Commit();
			outputBox.Enabled = false;
		}
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var doctors = doctorRepository.Get(includedProperties: [nameof(DoctorModel.DCT_User)], rowCount: 20);

		doctorTable.Update(doctors);
	}

	void OnBook(object? sender, EventArgs e)
	{
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
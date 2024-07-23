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
			.Disabled()
				.AddControl(new Label("Select a doctor to have an appointment with:"))
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

			doctorOptionsContainer.Enabled = false;
			appointmentSettingsContainer.Enabled = true;
        }
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();

		var patient = patientRepository.GetWhere(
			x => x.PAT_USR_ID == environment.CurrentUser.USR_PK,
			includedProperties: [nameof(PatientModel.PAT_Doctor)],
			rowCount: 1).SingleOrDefault();

		if (patient == null)
		{
			outputBox.Enabled = true;
			outputBox.SetState("Failed to retrieve patient information", OutputBox.OutputState.Error);
			return;
		}

		if (patient.PAT_Doctor == null)
		{
			doctorOptionsContainer.Enabled = true;
		}
		else
		{
			appointmentSettingsContainer.Enabled = true;
		}

		var doctors = doctorRepository.Get(includedProperties: [nameof(DoctorModel.DCT_User)], rowCount: 20);

		doctorTable.Update(doctors);
	}

	void OnBook(object? sender, EventArgs e)
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();

		var patient = patientRepository.GetWhere(
			x => x.PAT_USR_ID == environment.CurrentUser.USR_PK, 
			rowCount: 1,
			includedProperties: [nameof(PatientModel.PAT_Doctor)]).SingleOrDefault();

		if (patient == null)
		{
			outputBox.Enabled = true;
			outputBox.SetState("error occured retrieving patient info", OutputBox.OutputState.Error);
			return;
		}

		if (patient.PAT_Doctor == null)
		{
			outputBox.Enabled = true;
			outputBox.SetState("Patient has no doctor assigned", OutputBox.OutputState.Error);
			return;
		}

        if (inputDatetime == null)
		{
			outputBox.Enabled = true;
			outputBox.SetState("Please enter a valid datetime", OutputBox.OutputState.Error);
			return;
        }

		if (inputDatetime < DateTime.Now)
		{
			outputBox.Enabled = true;
			outputBox.SetState($"Appointment cannot be in the past, current date {DateTime.Now}", OutputBox.OutputState.Warn);
			return;
		}

		var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>();
        var appointment = new AppointmentModel()
		{
			APT_Doctor = patient.PAT_Doctor,
			APT_Patient = patient,
			APT_Description = inputDescription,
			APT_AppointmentTime = inputDatetime.Value,
		};
		appointmentRepository.Insert(appointment);

		outputBox.Enabled = true;
		outputBox.SetState($"Booked new appointment at {inputDatetime} with description: {inputDescription}", OutputBox.OutputState.Success);

        unitOfWork.Commit();
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
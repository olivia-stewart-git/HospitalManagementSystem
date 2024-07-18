using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class SpecificPatientView : View
{
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;
	OutputBox outputBox;
	TableView<PatientModel> tableView;

	int idInput;

	public SpecificPatientView(IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Check Patient Details"))
			.AddControl(new InputField("Enter the ID of the patient you want to check:", "patient-id-input"))
				.Setup<InputField>(x =>
				{
					x.BindProperty<int>(x => idInput = x, nameof(InputField.Contents));
                    x.Completed += HandleInput;
                })

			.AddControl(new OutputBox(string.Empty, "patient-output"))
				.Place(ref outputBox)

			.AddControl(new TableView<PatientModel>("Patients",
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_Email, overrideName: "Email Address"),
				new TableViewColumn<PatientModel>(x => x.PAT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<PatientModel>(x => $"{x.PAT_User.USR_Address_State}, {x.PAT_User.USR_Address_Postcode}, {x.PAT_User.USR_Address_Line1} {x.PAT_User.USR_Address_Line2}", overrideName: "Address")) { Enabled = false })
				.Place(ref tableView);
	}

	void HandleInput(object? sender, EventArgs e)
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();

		var patient = patientRepository.GetWhere(x => x.PAT_User.USR_ID == idInput,
			includedProperties: [nameof(PatientModel.PAT_User)]).FirstOrDefault();

		if (patient != null)
		{
			outputBox.Enabled = false;
			tableView.Enabled = true;
			tableView.Update([patient]);
		}
		else
		{
			outputBox.Enabled = true;
			tableView.Enabled = false;
            outputBox.SetState($"Could not find patient with ID {idInput}", OutputBox.OutputState.Warn);
		}

		viewService.Redraw();
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
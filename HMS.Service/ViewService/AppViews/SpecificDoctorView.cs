using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

/// <summary>
/// View specific doctor
/// If I had more time I would combine this and patient view into one
/// </summary>
public class SpecificDoctorView : View
{
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;
	OutputBox outputBox;
	TableView<DoctorModel> tableView;

	int idInput;

	public SpecificDoctorView(IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Check Doctor Details"))
			.AddControl(new InputField("Enter the ID of the Doctor you want to check:", "Doctor-id-input") { AllowOnlyNumeric = true })
			.Setup<InputField>(x =>
			{
				x.BindProperty<string>(b => int.TryParse(b, out idInput), nameof(InputField.Contents));
				x.Completed += HandleInput;
			})

			.AddControl(new OutputBox(string.Empty, "Doctor-output"))
			.Place(ref outputBox)

			.AddControl(new TableView<DoctorModel>("Doctors",
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_Email, overrideName: "Email Address"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<DoctorModel>(x => $"{x.DCT_User.USR_Address_State}, {x.DCT_User.USR_Address_Postcode}, {x.DCT_User.USR_Address_Line1} {x.DCT_User.USR_Address_Line2}", overrideName: "Address"))
				{ Enabled = false })
			.Place(ref tableView);
	}

	void HandleInput(object? sender, EventArgs e)
	{
		if (idInput == 0)
		{
			outputBox.Enabled = true;
			tableView.Enabled = false;
			outputBox.SetState($"Please Enter and ID", OutputBox.OutputState.Error);
			return;
		}

		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var DoctorRepository = unitOfWork.GetRepository<DoctorModel>();

		var Doctor = DoctorRepository.GetWhere(x => x.DCT_User.USR_ID == idInput,
			includedProperties: [nameof(DoctorModel.DCT_User)]).FirstOrDefault();

		if (Doctor != null)
		{
			outputBox.Enabled = false;
			tableView.Enabled = true;
			tableView.Update([Doctor]);
		}
		else
		{
			outputBox.Enabled = true;
			tableView.Enabled = false;
			outputBox.SetState($"Could not find Doctor with ID {idInput}", OutputBox.OutputState.Warn);
		}

		viewService.Redraw();
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
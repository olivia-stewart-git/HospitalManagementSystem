using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class DoctorDetailsView : View
{
	readonly IEnvironment environment;
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IViewService viewService;

	public DoctorDetailsView(IEnvironment environment, IUnitOfWorkFactory unitOfWorkFactory, IViewService viewService)
	{
		this.environment = environment;
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.viewService = viewService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "My Details"))
			.AddControl(new OutputBox(string.Empty, "doctor-details-outputBox") { Enabled = false })
			.AddControl(new TableView<DoctorModel>("Doctor Details",
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_Email, overrideName: "Email"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_PhoneNumber, overrideName: "Phone"),
                new TableViewColumn<DoctorModel>(x => $"{x.DCT_User.USR_Address_State}, {x.DCT_User.USR_Address_Postcode}, {x.DCT_User.USR_Address_Line1} {x.DCT_User.USR_Address_Line2}", overrideName: "Address")
                ));
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();

		var outputBox = Q<OutputBox>("doctor-details-outputBox");

		var detailsTable = Q<TableView<DoctorModel>>("Doctor Details");
		var doctor = doctorRepository
			.GetWhere(x => x.DCT_USR_ID == environment.CurrentUser.USR_PK, includedProperties: [nameof(DoctorModel.DCT_User)], rowCount: 1)
			.FirstOrDefault();

		if (doctor != null)
		{
			detailsTable.Update([doctor]);
		}
		else
		{
			detailsTable.Enabled = false;
			outputBox.Enabled = true;
			outputBox.SetState("Error occurred retrieving doctor information", OutputBox.OutputState.Error);
        }
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
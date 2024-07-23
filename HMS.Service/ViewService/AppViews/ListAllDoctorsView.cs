using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class ListAllDoctorsView : View
{
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	TableView<DoctorModel> tableView;
	OutputBox outputBox;

	public ListAllDoctorsView(IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "All Doctors on System"))
			.AddControl(new OutputBox(string.Empty, "doctor-details-outputBox"))
			.Disabled()
			.Place(ref outputBox)
			.AddControl(new TableView<DoctorModel>("Doctor Details",
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_Email, overrideName: "Email"),
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_PhoneNumber, overrideName: "Phone"),
				new TableViewColumn<DoctorModel>(x => $"{x.DCT_User.USR_Address_State}, {x.DCT_User.USR_Address_Postcode}, {x.DCT_User.USR_Address_Line1} {x.DCT_User.USR_Address_Line2}", overrideName: "Address")
				))
			.Place(ref tableView);
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var doctors = doctorRepository.Get(includedProperties: [nameof(DoctorModel.DCT_User)]).ToList();

		if (doctors.Count > 0)
		{
			tableView.Update(doctors);
		}
		else
		{
			outputBox.Enabled = true;
			outputBox.SetState("No Doctors found in system", OutputBox.OutputState.Warn);
			tableView.Enabled = false;
		}
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}
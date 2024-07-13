using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class DoctorDetailsView : View
{
	readonly IEnvironment environment;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	public DoctorDetailsView(IEnvironment environment, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.environment = environment;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "My Details"))
			.AddControl(new TableView<DoctorModel>("Doctor Details",
				new TableViewColumn<DoctorModel>(x => x.DCT_User.USR_FullName, overrideName: "Name")
				));
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var detailsTable = Q<TableView<DoctorModel>>("Doctor Details");
		var doctor = doctorRepository.GetById(environment.CurrentUser.USR_PK);

		if (doctor != null)
		{
			detailsTable.Update([doctor]);
		}
	}
}
using HMS.Data.DataAccess;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class AdministratorMenuView : View
{
	readonly IEnvironment environment;

	public AdministratorMenuView(IEnvironment environment)
	{
		this.environment = environment;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Administrator Menu"))
			.AddControl(new Label($"Welcome to DOTNET Hospital Management System {environment.CurrentUser.USR_FullName}"))
			.AddControl(new NewLine());
	}
}
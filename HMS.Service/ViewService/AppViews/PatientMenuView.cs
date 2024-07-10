using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class PatientMenuView : View
{
	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Patient Menu"));
	}
}
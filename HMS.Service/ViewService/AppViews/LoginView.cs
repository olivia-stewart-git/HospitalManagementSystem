using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class LoginView : View
{
	readonly ILogonService logonService;

	public LoginView(ILogonService logonService)
	{
		this.logonService = logonService;
	}

	public override void OnBecomeActive()
	{
		logonService.StartLogonProcess();
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Login"))
			.AddControl(new OutputBox("Output", "login-output") { Enabled = false })
	        .AddControl(new Label("ID:"))
	        .AddControl(new Label("Password:"));
	}
}
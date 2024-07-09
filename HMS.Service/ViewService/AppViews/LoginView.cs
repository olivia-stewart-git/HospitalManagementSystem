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
	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.WithControl(new PageHeader("DOTNET Hospital Management System", "Login"))
			.WithControl(new OutputBox("Output", "login-output") { Enabled = false })
	        .WithControl(new Label("ID:"))
	        .WithControl(new Label("Password:"));
	}
}
﻿using HMS.Data.DataAccess;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class PatientMenuView : View
{
	readonly IEnvironment environment;

	public PatientMenuView(IEnvironment environment)
	{
		this.environment = environment;
	}

    public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Patient Menu"))
			.AddControl(new Label($"Welcome to DOTNET Hospital Management System {environment.CurrentUser.USR_FullName}"))
			.AddControl(new NewLine());
	}
}
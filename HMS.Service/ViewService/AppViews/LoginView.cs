﻿using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class LoginView : View
{
	readonly ILogonService logonService;

	int userIdCurrentValue;
	string userIdCurrentPassword;
	OutputBox outputBox;

	public LoginView(ILogonService logonService)
	{
		this.logonService = logonService;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Login"))
			.AddControl(new OutputBox("Output", "login-output") { Enabled = false })
			.AddControl(new InputField("ID: ", "login-input-id") { AllowOnlyNumeric = true })
			.AddControl(new InputField("Password: ", "login-input-password") { ObscureContent = true })
			.AddControl(new NewLine())
			.AddControl(new InteractionOption("Login", "login-enter-option"));
	}

    public override void OnBecomeActive()
    {
	    outputBox = Q<OutputBox>("login-output");

		var enterOption = Q<InteractionOption>("login-enter-option");
		enterOption.Interacted += OnLoginPressed;

		var userIdInput = Q<InputField>("login-input-id");
		userIdInput.BindProperty<string>(x =>
			{
				if (int.TryParse(x, out var value))
				{
					userIdCurrentValue = value;
				}
			},
			nameof(userIdInput.Contents));

		var passwordInput = Q<InputField>("login-input-password");
		passwordInput.BindProperty<string>(x => userIdCurrentPassword = x, nameof(passwordInput.Contents));
	}

	void SetInteractable()
	{
	}

	void OnLoginPressed(object? sender, EventArgs e)
	{
		if (!logonService.ExecuteLogin(userIdCurrentValue, userIdCurrentPassword))
		{
			outputBox.Enabled = true;
			outputBox.SetState("Incorrect Login Details", OutputBox.OutputState.Error);
		};
	}
}
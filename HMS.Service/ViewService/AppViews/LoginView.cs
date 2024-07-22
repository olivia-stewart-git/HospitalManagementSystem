using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class LoginView : View
{
	readonly ILogonService logonService;

	int userIdCurrentValue;
	string userIdCurrentPassword = string.Empty;
	OutputBox outputBox;
	Button enterOption;

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
			.AddControl(new Button("Login", "login-enter-option"));
	}

    public override void OnBecomeActive()
    {
	    outputBox = Q<OutputBox>("login-output");

		enterOption = Q<Button>("login-enter-option");
		enterOption.Interacted += OnLoginPressed;

		var userIdInput = Q<InputField>("login-input-id");
		userIdInput.BindProperty<string>(x =>
			{
				if (int.TryParse(x, out var value))
				{
					userIdCurrentValue = value;
				}
				SetInteractable();
			},
			nameof(userIdInput.Contents));

		var passwordInput = Q<InputField>("login-input-password");
		passwordInput.BindProperty<string>(x =>
		{
			userIdCurrentPassword = x;
			SetInteractable();
		}, nameof(passwordInput.Contents));
	}

	void SetInteractable()
	{
		if (!string.IsNullOrEmpty(userIdCurrentPassword) && userIdCurrentValue > 0)
		{
			enterOption.IsInteractable = true;
		}
		else
		{
			enterOption.IsInteractable = false;
		}
	}

	void OnLoginPressed(object? sender, EventArgs e)
	{
		if (!logonService.ExecuteLogin(userIdCurrentValue, userIdCurrentPassword))
		{
			outputBox.Enabled = true;
			outputBox.SetState("Incorrect Login Details", OutputBox.OutputState.Error);
		}
		else
		{
			outputBox.Enabled = true;
			outputBox.SetState("Successful Login", OutputBox.OutputState.Success);
        }
	}
}
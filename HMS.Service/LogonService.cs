using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.Interaction;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using HMS.Service.ViewService.Controls;

namespace HMS.Service;

public class LogonService : ILogonService
{
	readonly IInputService inputService;
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	public bool IsLoggedIn { get; private set; }
	public Guid CurrentUser { get; private set; }
	public bool DoRepeatLogon { get; init; } = true;

	public LogonService(IInputService inputService, IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.inputService = inputService;
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public void StartLogonProcess()
	{
		var loginView = viewService.CurrentView;
		var outputBox = loginView.Q<OutputBox>("login-output");
		PromptLogon(outputBox);
	}

	void PromptLogon(OutputBox? outputBox)
	{
		var logonValues = ReadLogonValues();

		if (TryValidateLogon(logonValues.userId, logonValues.password, out var user))
		{
			ExecuteLogon(user);
			return;
		}

		if (outputBox != null)
		{
			outputBox.Enabled = true;
			outputBox.SetState("Incorrect Login Details", OutputBox.OutputState.Error);
		}

		if (DoRepeatLogon)
		{
			PromptLogon(outputBox);
		}
	}

	(int userId, string password) ReadLogonValues()
	{
		var userId = inputService.ReadIntegerInput();
		var password = inputService.ReadInput();

		return (userId, password);
	}

	bool TryValidateLogon(int userId, string password, out UserModel? user)
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var userRepository = unitOfWork.GetRepository<UserModel>();

		var targetUser = userRepository.GetWhere(x => x.USR_ID == userId, 1).FirstOrDefault();
		if (targetUser == null || targetUser.USR_Password != password)
		{
			user = null;
			return false;
		}

		user = targetUser;
        return true;
	}

	void ExecuteLogon(UserModel user)
	{
		IsLoggedIn = true;
		CurrentUser = user.USR_PK;
	}
}
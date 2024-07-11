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
	readonly IEnvironment environment;

	public bool IsLoggedIn { get; private set; }
	public bool DoRepeatLogon { get; init; } = true;

	public LogonService(IInputService inputService, IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory, IEnvironment environment)
	{
		this.inputService = inputService;
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
		this.environment = environment;
	}

	public bool ExecuteLogin(int userId, string password)
	{
		if (TryValidateLogon(userId, password, out var user))
		{
			ExecuteLogon(user);
			return true;
		}

		return false;
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
		var role = user.GetRole(unitOfWorkFactory);
		environment.CurrentUser = user;
		environment.CurrentRole = role;
        LoadApp(role);
	}

	void LoadApp(SystemRole role)
	{
		switch (role)
		{
			case SystemRole.None:
                break;
			case SystemRole.Doctor:
				viewService.SwitchView<DoctorMenuView>();
                break;
			case SystemRole.Admin:
				viewService.SwitchView<AdministratorMenuView>();
				break;
			case SystemRole.Patient:
				viewService.SwitchView<PatientMenuView>();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
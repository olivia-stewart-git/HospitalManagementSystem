using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.Interaction;
using HMS.Service.ViewService;

namespace HMS.Service;

public class LogonService : ILogonService
{
	readonly IInputService inputService;
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	public bool IsLoggedIn { get; private set; }
	public Guid CurrentLoggedOnUserId { get; private set; }

	public LogonService(IInputService inputService, IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.inputService = inputService;
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public void StartLogonProcess()
	{
		var logonValues = ReadLogonValues();

		if (TryValidateLogon(logonValues.userId, logonValues.password, out var user))
		{
			ExecuteLogon(user);
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
		using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
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
		CurrentLoggedOnUserId = user.USR_PK;
	}
}
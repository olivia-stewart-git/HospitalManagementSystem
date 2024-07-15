using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.Interaction;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using HMS.Service.ViewService.Controls;

namespace HMS.Service;

public class LogonService : ILogonService
{
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;
	readonly IEnvironment environment;

	public bool IsLoggedIn { get; private set; }
	public bool DoRepeatLogon { get; init; } = true;

	public LogonService(IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory, IEnvironment environment)
	{
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

#if DEBUG
		if (TryGetTestOverrideForUser(unitOfWork, password, out var newUser))
		{
			user = newUser;
			return true;
		}
#endif

        var targetUser = userRepository.GetWhere(x => x.USR_ID == userId, 1).FirstOrDefault();
		if (targetUser == null || targetUser.USR_Password != password)
		{
			user = null;
			return false;
		}

		user = targetUser;
        return true;
	}

#if DEBUG
	bool TryGetTestOverrideForUser(IUnitOfWork unitOfWork, string password, out UserModel? overrideUser)
	{
		var userRepository = unitOfWork.GetRepository<UserModel>();
		overrideUser = null;
		switch (password)
		{
			case "testDoctor":
				var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
				var targetUserId = doctorRepository.Get(1).First().DCT_USR_ID;

                overrideUser = userRepository.GetById(targetUserId) 
	                ?? throw new InvalidOperationException("Could not find override doctor");
				return true;
			case "testAdmin":
				var adminRepository = unitOfWork.GetRepository<AdministratorModel>();
				var targetAdminUserId = adminRepository.Get(1).First().ADM_USR_ID;

				overrideUser = userRepository.GetById(targetAdminUserId)
					?? throw new InvalidOperationException("Could not find override admin");
                return true;
			case "testPatient":
				var patientRepository = unitOfWork.GetRepository<PatientModel>();
				var targetPatientId = patientRepository.Get(1).First().PAT_USR_ID;

				overrideUser = userRepository.GetById(targetPatientId)
					?? throw new InvalidOperationException("Could not find override patient");
				return true;
		}
		return false;
	}
#endif

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
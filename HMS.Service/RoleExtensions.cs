using HMS.Data.DataAccess;
using HMS.Data.Models;

namespace HMS.Service;

public static class RoleExtensions
{
	public static SystemRole GetRole(this UserModel userModel, IUnitOfWorkFactory unitOfWorkFactory)
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		if (unitOfWork.GetRepository<DoctorModel>().Exists(x => x.DCT_USR_ID == userModel.USR_PK))
		{
			return SystemRole.Doctor;
		}

		if (unitOfWork.GetRepository<PatientModel>().Exists(x => x.PAT_USR_ID == userModel.USR_PK))
		{
			return SystemRole.Patient;
		}

		if (unitOfWork.GetRepository<AdministratorModel>().Exists(x => x.ADM_USR_ID == userModel.USR_PK))
		{
			return SystemRole.Admin;
		}
		return SystemRole.None;
	}
}
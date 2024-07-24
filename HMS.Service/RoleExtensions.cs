using HMS.Data.DataAccess;
using HMS.Data.Models;

namespace HMS.Service;

/// <summary>
/// Extensions methods relating to system roles
/// </summary>
public static class RoleExtensions
{
    /// <summary>
    /// Retrieve the role of a user based on its related tables
    /// </summary>
    /// <param name="userModel"></param>
    /// <param name="unitOfWorkFactory"></param>
    /// <returns></returns>
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
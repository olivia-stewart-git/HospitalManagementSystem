using HMS.Data.DataAccess;
using HMS.Data.Models;

namespace HMS.Data;

/// <summary>
/// Seeds data from csv file generated using Mockaroo.com
/// </summary>
public class Seeder : ISeeder
{
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	public Seeder(IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public void SeedDb()
	{
		if (ShouldSeed())
		{
			DoSeed();
		}
	}

	void DoSeed()
	{
		SeedUsers();
		SeedDoctors();
		SeedPatients();
		SeedAdministrators();
		SeedAppointments();
		SeedStmData();
    }

	bool ShouldSeed()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var stmDataRepository = unitOfWork.GetRepository<STMDataModel>();

		var stmData = stmDataRepository.Get(1);
		if (stmData.Any())
		{
			return !stmData.First().STM_HasSeeded;
		}

		return true;
	}

	void SeedUsers()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var userRepository = unitOfWork.GetRepository<UserModel>();

		var users = SeedingDataRepository.ConsumeUsers();
		foreach (var userModel in users)
		{
			userRepository.Insert(userModel);
		}

		unitOfWork.Commit();
	}

	void SeedDoctors()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var userRepository = unitOfWork.GetRepository<UserModel>();

		var doctors = new List<DoctorModel>();

		var existingUsers = userRepository.GetWhere(x => x.USR_ID <= 300);
		foreach (var userModel in existingUsers)
		{
			doctors.Add(new DoctorModel()
			{
				DCT_PK = Guid.NewGuid(),
				DCT_User = userModel,
			});
        }

		doctorRepository.InsertRange(doctors);
		unitOfWork.Commit();
	}

	void SeedPatients()
	{
		var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();
		var userRepository = unitOfWork.GetRepository<UserModel>();

		var doctors = doctorRepository.Get().ToList();

		var random = new Random();

		var patients = new List<PatientModel>();
        var existingUsers = userRepository.GetWhere(x => x.USR_ID > 300 && x.USR_ID <= 900);
		foreach (var userModel in existingUsers)
		{
			var rand = random.Next(doctors.Count);
			var targetDoctor = doctors[rand];

            patients.Add(new PatientModel()
			{
				PAT_PK = Guid.NewGuid(),
				PAT_User = userModel,
				PAT_Doctor = targetDoctor,
            });
		}
		patientRepository.InsertRange(patients);
		unitOfWork.Commit();
	}

	void SeedAdministrators()
	{
		var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
		var administratorRepository = unitOfWork.GetRepository<AdministratorModel>();
		var userRepository = unitOfWork.GetRepository<UserModel>();

        var administratorModels = new List<AdministratorModel>();
		var existingUsers = userRepository.GetWhere(x => x.USR_ID > 900 && x.USR_ID <= 1000);
		foreach (var userModel in existingUsers)
		{
			administratorModels.Add(new AdministratorModel()
			{
				ADM_PK = Guid.NewGuid(),
				ADM_User = userModel,
			});
		}
		administratorRepository.InsertRange(administratorModels);
		unitOfWork.Commit();
	}

	void SeedAppointments()
	{
		var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();
		var doctorRepository = unitOfWork.GetRepository<DoctorModel>();
		var appointmentRepository = unitOfWork.GetRepository<AppointmentModel>();

        var doctors = doctorRepository.Get().ToList();
		var patients = patientRepository.Get().OrderBy(x => Guid.NewGuid()).ToList(); //Randomly order patients

		var rand = new Random();

        for (int i = 0; i < doctors.Count; i++)
        {
	        var patientCount = rand.Next(1, 10);
	        var doctor = doctors[i];
	        var startIndex = rand.Next(patients.Count - 1 - patientCount);
	        for (int j = startIndex; j < startIndex + patientCount; j++)
	        {
				var patient = patients[j];
				doctor.DCT_Patients.Add(patient);
				patient.PAT_Doctor = doctor;
				doctorRepository.Update(doctor);
				patientRepository.Update(patient);

				var appointment = new AppointmentModel()
				{
					APT_PK = Guid.NewGuid(),
					APT_AppointmentTime = SeedingDataRepository.GetRandomAppointmentDate(),
					APT_Description = SeedingDataRepository.GetRandomAppointmentDescription(),
					APT_Doctor = doctor,
					APT_Patient = patient,
				};
				appointmentRepository.Insert(appointment);
	        }
        }
		unitOfWork.Commit();
	}

	//This is just data to record whether we have seeded before so we don't do it everytime we open.
    void SeedStmData()
    {
	    var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
	    var stmDataRepository = unitOfWork.GetRepository<STMDataModel>();
        var data = new STMDataModel()
		{
			STM_PK = Guid.NewGuid(),
			STM_HasSeeded = true,
		};
		stmDataRepository.Insert(data);
		unitOfWork.Commit();
	}
}
using HMS.Data.Models;

namespace HMS.Data;

public class Seeder : ISeeder
{
	readonly HMSDbContext dbContext;

	public Seeder(HMSDbContext dbContext)
	{
		this.dbContext = dbContext;
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
		SeedStmData();
    }

	bool ShouldSeed()
	{
		var stmData = dbContext.StmData.Take(1);
		if (stmData.Any())
		{
			return !stmData.First().STM_HasSeeded;
		}

		return true;
	}

	void SeedUsers()
	{
		var users = SeedingDataRepository.ConsumeUsers();
		foreach (var userModel in users)
		{
			dbContext.Users.Add(userModel);
		}

		dbContext.SaveChanges();
	}

	void SeedDoctors()
	{
		var doctors = new List<DoctorModel>();
		var existingUsers = dbContext.Users.Where(x => x.USR_ID <= 300);
		foreach (var userModel in existingUsers)
		{
			doctors.Add(new DoctorModel()
			{
				DCT_PK = Guid.NewGuid(),
				DCT_User = userModel,
			});
        }
		dbContext.Doctors.AddRange(doctors);
		dbContext.SaveChanges();
	}

	void SeedPatients()
	{
		var doctors = dbContext.Doctors.ToList();
		var random = new Random();

		var patients = new List<PatientModel>();
        var existingUsers = dbContext.Users.Where(x => x.USR_ID > 300 && x.USR_ID < 900);
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
		dbContext.Patients.AddRange(patients);
		dbContext.SaveChanges();
	}

	void SeedAdministrators()
	{
		var administratorModels = new List<AdministratorModel>();
		var existingUsers = dbContext.Users.Where(x => x.USR_ID > 300 && x.USR_ID < 900);
		foreach (var userModel in existingUsers)
		{
			administratorModels.Add(new AdministratorModel()
			{
				ADM_PK = Guid.NewGuid(),
				ADM_User = userModel,
			});
		}
		dbContext.Administrators.AddRange(administratorModels);
		dbContext.SaveChanges();
	}

    void SeedStmData()
	{
		var data = new STMDataModel()
		{
			STM_PK = Guid.NewGuid(),
			STM_HasSeeded = true,
		};
		dbContext.StmData.Add(data);
		dbContext.SaveChanges();
	}
}
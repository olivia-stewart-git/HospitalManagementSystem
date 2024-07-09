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
		SeedStmData();
    }

	bool ShouldSeed()
	{
		var stmData = dbContext.StmData.Take(1);
		if (stmData.Any())
		{
			return !stmData.First().STM_HasSeeded;
		}

		return false;
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
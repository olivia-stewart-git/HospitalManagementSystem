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
		SeedUsers();
	}

	void SeedUsers()
	{

	}
}
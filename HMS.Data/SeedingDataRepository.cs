using HMS.Data.Models;
using System.Reflection;

namespace HMS.Data;

/// <summary>
/// Helper class to retrieve seeding data
/// </summary>
public class SeedingDataRepository
{
	//we consume embedded resource csv file.
	public static IEnumerable<UserModel> ConsumeUsers()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using Stream stream = assembly.GetManifestResourceStream("HMS.Data.Resources.MockUserData.csv")
			?? throw new InvalidOperationException("Could not find seeding resource");

		string[] states = ["NSW", "ACT", "VIC", "WA", "TAS"];
		var rand = new Random();
		var users = new List<UserModel>();
		using StreamReader reader = new StreamReader(stream);
		int i = 0;
		while (!reader.EndOfStream)
		{
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			i++;
            var model = new UserModel
            {
                USR_PK = Guid.NewGuid(),
				USR_Password = $"userpassword_{values[0]}",
				USR_FirstName = values[0],
				USR_LastName = values[1],
				USR_Email = values[2],
				USR_Address_Line2 = values[3],
				USR_Address_Line1 = values[4],
				USR_Address_State = states[rand.Next(states.Length)],
				USR_Address_Postcode = values[6],
                USR_PhoneNumber = values[7],
				USR_ID = i,
            };

			users.Add(model);
		}
		return users;
	}

	static readonly List<string> Descriptions =
    [
        "Usual checkup",
		"Concerns about chest pain",
		"Follow-up visit",
		"Prescription renewal",
		"Routine blood test",
		"Consultation for surgery",
		"Skin rash evaluation",
		"Allergy testing",
		"Mental health consultation",
		"Physical therapy session",
		"Vaccination appointment",
		"Diabetes management",
		"Dietary consultation",
		"Heart rate monitoring",
		"Back pain evaluation"
	];

	static readonly Random RandomGenerator = new ();

	public static string GetRandomAppointmentDescription()
	{
		int index = RandomGenerator.Next(Descriptions.Count);
		return Descriptions[index];
	}

	public static DateTime GetRandomAppointmentDate()
	{
		var startDate = DateTime.Now.AddMonths(-6);
		var endDate = DateTime.Now.AddMonths(6);

		var range = (endDate - startDate).Days;
		
		startDate = startDate.AddDays(RandomGenerator.Next(range));
		startDate = startDate.AddHours(RandomGenerator.Next(12));
		startDate = startDate.AddSeconds(RandomGenerator.Next(60));
        return startDate;
	}
}
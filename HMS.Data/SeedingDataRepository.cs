using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HMS.Data.Models;

namespace HMS.Data;

public class SeedingDataRepository
{
	public static IEnumerable<UserModel> ConsumeUsers()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using Stream stream = assembly.GetManifestResourceStream("HMS.Data.Resources.MockUserData.csv");
		if (stream == null)
		{
			return [];
		}

		var users = new List<UserModel>();
		var id = 0;
		using StreamReader reader = new StreamReader(stream);
		while (!reader.EndOfStream)
		{
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			id++;
            var model = new UserModel
            {
                USR_PK = Guid.NewGuid(),
				USR_Password = "userpassword_" + id,
                USR_ID = id,
				USR_FirstName = values[0],
				USR_LastName = values[1],
				USR_Email = values[2],
				USR_Address_Line2 = values[3],
				USR_Address_Line1 = values[4],
				USR_Address_State = "NSW",
				USR_Address_Postcode = values[6],
                USR_PhoneNumber = values[7],
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
		return startDate.AddDays(RandomGenerator.Next(range));
    }
}
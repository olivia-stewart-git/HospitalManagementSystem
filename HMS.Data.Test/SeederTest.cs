using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Test;

internal class SeederTest
{
	[Test]
	public void TestSeeder()
	{
		var users = SeedingDataRepository.ConsumeUsers().ToList();
		Assert.That(users, Has.Count.EqualTo(1000));
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.ViewService.Test;

internal class ViewTest
{
	[Test]
	public void TestAllViewsAreConstructable()
	{
		var viewTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(type => typeof(View).IsAssignableFrom(type))
			.Where(x => x.Name != "View");


		void Assertion()
		{
			foreach (var viewType in viewTypes)
			{
				Assert.DoesNotThrow(() => Activator.CreateInstance(viewType));
			}
		}
		Assert.Multiple(Assertion);
	}
}
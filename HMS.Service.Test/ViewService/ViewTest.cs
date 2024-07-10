using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HMS.Service.ViewService.Controls;

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
				Assert.That(viewType.GetConstructors(BindingFlags.Public), Has.Length.LessThan(2));
			}
		}
		Assert.Multiple(Assertion);
	}

	[Test]
	public void TestQueryForControl()
	{
		//Arrange
		var view = new TestView();
		var expectedLabel = new Label("some label", "TestQuery");

        view.AddControl(expectedLabel);

		//Act
		var result = view.Q<Label>("TestQuery");

		//Assert
		Assert.That(result, Is.EqualTo(expectedLabel));
	}
}
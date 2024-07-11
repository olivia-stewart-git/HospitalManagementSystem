using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Service.TestUtilities;

namespace HMS.Service.ViewService.Controls.Test;

public class ViewControlTest
{
	[Test]
	public void TestDataBinding()
	{
		//Arrange
		var control = new Label("testValue");

		var testVariable = string.Empty;

		//Act
		control.BindProperty<string>(x => testVariable = x, nameof(control.Text));

		//Assert
		Assert.That(testVariable, Is.EqualTo("testValue"));
		control.Text = "newValue";
		Assert.That(testVariable, Is.EqualTo("newValue"));
    }
}
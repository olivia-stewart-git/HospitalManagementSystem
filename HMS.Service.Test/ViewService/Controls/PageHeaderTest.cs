using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.ViewService.Controls.Test;

internal class PageHeaderTest
{
	[Test]
	public void TestHeader()
	{
		//Arrange
		var title = "Test Header";
		var subtitle = "Test Subtitle";

        var header = new PageHeader("Test Header", "Test Subtitle");

		//Act
		var result = header.Render();
		
		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(result[0].Contents.Contains(title), Is.EqualTo(true));
			Assert.That(result[0].Contents.Contains(subtitle), Is.EqualTo(true));
        });
	}
}
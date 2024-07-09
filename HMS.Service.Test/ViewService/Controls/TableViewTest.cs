using HMS.Service.TestUtilities;

namespace HMS.Service.ViewService.Controls.Test;

internal class TableViewTest
{
	[Test]
	public void TestTableViewRenders()
	{
		//Arrange
		var model = new TableView<TestModel>("TestView", new (x => x.Name), new(x => x.Description));
		var data = new[]
		{
			new TestModel()
			{
				Name = "My Test Name",
				Description = "Some Description",
			},
			new TestModel()
			{
				Name = "Another one",
				Description = "Cool one we like",
			}
        };
		model.Update(data);

		//Act
		var render = model.Render();

		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(render.Contents.Contains("Name"), Is.True);
			Assert.That(render.Contents.Contains("Description"), Is.True);
            Assert.That(render.Contents.Contains(data[0].Name), Is.True);
			Assert.That(render.Contents.Contains(data[1].Name), Is.True);
			Assert.That(render.Contents.Contains(data[0].Description), Is.True);
			Assert.That(render.Contents.Contains(data[1].Description), Is.True);
        });
	}
}
using HMS.Data.DataAccess;
using HMS.Service.TestUtilities;

namespace HMS.Service.ViewService.Controls.Test;

internal class TableViewTest
{
	[Test]
	public void TestTableViewRenders()
	{
		//Arrange
		var model = new TableView<TestModel>("TestView", new (x => x.Name), new(x => x.Description)) { CullPropertyPrefix = false };
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
			},
			new TestModel()
			{
				Name = "This wraps",
				Description = "Blah blah we want this one to have lots of text so it wraps in the cell yo!",
			}
        };
		model.Update(data);

		//Act
		var render = model.Render();
		var completeRenderContents = string.Join(System.Environment.NewLine, render.Select(x => x.Contents));

		//Assert
		Assert.Multiple(() =>
		{
			Assert.That(completeRenderContents.Contains("Name"), Is.True);
			Assert.That(completeRenderContents.Contains("Description"), Is.True);
            Assert.That(completeRenderContents.Contains(data[0].Name), Is.True);
			Assert.That(completeRenderContents.Contains(data[1].Name), Is.True);
			Assert.That(completeRenderContents.Contains(data[0].Description), Is.True);
			Assert.That(completeRenderContents.Contains(data[1].Description), Is.True);
        });
	}

	[Test]
	public void TestBindsData()
	{
		//Arrange
		var propagator = new TestChangePropagator() { 
			Name = "First Propagator", 
			Values = [
				new TestModel() { Name = "Bob"},
				new TestModel() { Name = "Antony" },
            ]
        };

		var table = new TableView<TestModel>("table", new TableViewColumn<TestModel>(x => x.Name));
		table.Update(propagator.Values);
		table.Bind(propagator);

		//Pre assertions
		var value = table.Render();
		var completeRenderContents = string.Join(System.Environment.NewLine, value.Select(x => x.Contents));
        Assert.That(completeRenderContents.Contains("Bob"), Is.EqualTo(true));

		//Act
		propagator.Values = [new TestModel() { Name = "Mark" }];
		var reRenderValue = table.Render();
		var reRenderContents = string.Join(System.Environment.NewLine, reRenderValue.Select(x => x.Contents));
        Assert.That(reRenderContents.Contains("Mark"), Is.EqualTo(true));

    }
}
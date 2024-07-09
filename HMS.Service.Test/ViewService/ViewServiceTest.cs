using Moq;
using System.Text;

namespace HMS.Service.ViewService.Test
{
    public class ViewServiceTest
    {
	    [Test]
	    public void TestViewHasCorrectContent()
	    {
			//Arrange
			var viewService = new ViewService(Mock.Of<IViewWriter>());

			//Act
			var newView = viewService.SwitchView<TestView>();
			var value = newView.Render();

			//Assert
			Assert.That(value.Any(x => x.Contents.Contains("My Test View!")), Is.True);
			Assert.That(viewService.CurrentView, Is.EqualTo(newView));
	    }

	    [Test]
	    public void TestViewWritesToConsole()
	    {
		    //Arrange
			var sb = new StringBuilder();
			var viewWriter = new Mock<IViewWriter>();
			viewWriter.Setup(x => x.WriteLine(It.IsAny<RenderElement>())).Callback<RenderElement>(msg => sb.AppendLine(msg.Contents));

		    var viewService = new ViewService(viewWriter.Object);

		    //Act
		    viewService.SwitchView<TestView>();

			Assert.That(sb.ToString().Trim(Environment.NewLine.ToCharArray()), Is.EqualTo("My Test View!"));
	    }

	    [Test]
	    public void TestUnloadsPriorView()
	    {
			//Arrange
		    var viewService = new ViewService(Mock.Of<IViewWriter>());
		    var currentView = viewService.SwitchView<TestView>();

		    bool didCallback = false;
		    currentView.Unloaded += (_, _) => didCallback = true;

		    //Act
		    viewService.SwitchView<TestView>();

			//Assert
			Assert.That(didCallback, Is.True);
	    }
    }
}

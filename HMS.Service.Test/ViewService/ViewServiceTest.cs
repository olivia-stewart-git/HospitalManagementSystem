using HMS.Service.ViewService.Controls;
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
			Assert.That(value, Is.EqualTo("My Test View!"));
			Assert.That(viewService.CurrentView, Is.EqualTo(newView));
	    }

	    [Test]
	    public void TestViewWritesToConsole()
	    {
		    //Arrange

			var sb = new StringBuilder();
			var viewWriter = new Mock<IViewWriter>();
			viewWriter.Setup(x => x.Write(It.IsAny<string>())).Callback<string>(msg => sb.AppendLine(msg));

		    var viewService = new ViewService(viewWriter.Object);

		    //Act
		    viewService.SwitchView<TestView>();

			Assert.That(sb.ToString().Trim(Environment.NewLine.ToCharArray()), Is.EqualTo("My Test View!"));
	    }

	    [Test]
	    public void TestUnloadsPriorView()
	    {

	    }
    }

    public class TestView : View
    {
	    public override void BuildView(ViewBuilder viewBuilder)
	    {
		    viewBuilder.WithControl(new Label("My Test View!"));
	    }

	    public override void OnUnload()
	    {
	    }
    }
}

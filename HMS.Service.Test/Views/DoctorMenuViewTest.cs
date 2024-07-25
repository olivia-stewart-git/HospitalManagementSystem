using HMS.Data.Models;
using HMS.Service.ViewService;
using HMS.Service.ViewService.AppViews;
using HMS.Service.ViewService.Controls;
using Moq;

namespace HMS.Service.Views.Test;

internal class DoctorMenuViewTest
{
	[Test]
	public void TestHasCurrentUserName()
	{
		var mockEnvironment = new Mock<IEnvironment>();
		var user = new UserModel() { USR_FirstName = "Test", USR_LastName = "LastName" };
		mockEnvironment.Setup(x => x.CurrentUser).Returns(user);
		var doctorMenuView = new DoctorMenuView(mockEnvironment.Object, Mock.Of<IViewService>(), Mock.Of<ILogonService>());

		var builder = new ViewBuilder(doctorMenuView);
		doctorMenuView.BuildView(builder);
		builder.Build();

		var titleField = doctorMenuView.Q<Label>("main-label");
		Assert.NotNull(titleField);
		Assert.That(titleField.Text.Contains(user.USR_FullName), Is.True);
    }
}
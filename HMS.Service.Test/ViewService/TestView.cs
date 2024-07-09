using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.Test;

public class TestView : View
{
	public EventHandler Unloaded { get; set; }

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder.WithControl(new Label("My Test View!"));
	}

	public override void OnUnload()
	{
		Unloaded?.Invoke(this, EventArgs.Empty);
	}
}
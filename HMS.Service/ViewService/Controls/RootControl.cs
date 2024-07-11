namespace HMS.Service.ViewService.Controls;

public class RootControl : ViewControl
{
	public RootControl(string name) : base(name)
	{
	}

	public override List<RenderElement> Render()
	{
		return [RenderElement.Empty];
	}
}
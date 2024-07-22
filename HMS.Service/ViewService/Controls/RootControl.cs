namespace HMS.Service.ViewService.Controls;

public class RootControl : ViewControl
{
	public RootControl(string name) : base(name)
	{
	}

	protected override List<RenderElement> OnRender()
	{
		return [RenderElement.Empty];
	}
}
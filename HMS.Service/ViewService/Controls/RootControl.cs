namespace HMS.Service.ViewService.Controls;

/// <summary>
/// The root control that holds all children on a view
/// </summary>
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
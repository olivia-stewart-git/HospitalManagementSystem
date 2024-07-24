namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Control that acts as a parent container to other controls.
/// Useful if you want to enable/disable a large amount of controls at a time.
/// </summary>
public class ControlContainer : ViewControl
{
	public ControlContainer(string name, params ViewControl[] controls) : this(name, controls.ToList())
	{
	}

	public ControlContainer(string name, List<ViewControl> children) : base(name)
	{
		this.Children = children;
	}

    protected override List<RenderElement> OnRender()
	{
		return [RenderElement.Empty];
    }
}
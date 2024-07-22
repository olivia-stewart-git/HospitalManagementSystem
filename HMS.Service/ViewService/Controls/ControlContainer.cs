namespace HMS.Service.ViewService.Controls;

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
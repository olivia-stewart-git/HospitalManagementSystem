namespace HMS.Service.ViewService.Controls;

public class ControlContainer : ViewControl
{
	readonly List<ViewControl> children;

	public ControlContainer(string name, params ViewControl[] controls) : this(name, controls.ToList())
	{
	}

	public ControlContainer(string name, List<ViewControl> children) : base(name)
	{
		this.children = children;
	}

	public void AddChild(ViewControl control)
	{
		children.Add(control);
	}

    public override List<RenderElement> Render()
	{
		if (!Enabled)
		{
			return [RenderElement.Empty];
		}
		return children.SelectMany(x => x.Render()).ToList();
	}
}
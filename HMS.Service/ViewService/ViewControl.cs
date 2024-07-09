namespace HMS.Service.ViewService;

public abstract class ViewControl
{
	public string Name { get; }
	public bool Enabled { get; set; } = true;

    protected ViewControl(string name)
	{
		Name = name;
	}

	public abstract RenderElement Render();
}
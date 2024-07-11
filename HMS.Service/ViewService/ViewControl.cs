using HMS.Common;

namespace HMS.Service.ViewService;

public abstract class ViewControl : IChangePropagator<ViewControl>
{
	bool enabled = true;
	public string Name { get; }

	public bool Focused { get; protected set; } = false;

	public int YPosition { get; set; } = 0;

	public bool Enabled
	{
		get => enabled;
		set
		{
			OnChange?.Invoke(this, this);
			enabled = value;
		}
	}

	public EventHandler<ViewControl> OnChange { get; set; }
	public void DoChange()
	{
		OnChange?.Invoke(this, this);
	}

	protected ViewControl(string name)
	{
		Name = name;
	}

	public abstract RenderElement Render();
}
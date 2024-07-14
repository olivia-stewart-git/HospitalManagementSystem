namespace HMS.Service.ViewService;

public class ViewBuilder
{
	readonly View targetView;
	readonly List<ViewControl> controls = [];

	ViewControl? lastControl;

	public ViewBuilder(View targetView)
	{
		this.targetView = targetView;
	}

	public ViewBuilder AddControl(ViewControl control)
	{
		controls.Add(control);
		return this;
	}

	public void Setup<T>(Action<T> controlAction) where T : ViewControl
	{
		if (lastControl != null)
		{
			controlAction((lastControl as T)
				?? throw new InvalidOperationException("Could not cast to specified view control type"));
		}
	}

	public List<ViewControl> Build()
	{
		targetView.Controls = controls;
		return controls;
	}
}
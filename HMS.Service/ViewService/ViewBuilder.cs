namespace HMS.Service.ViewService;

public class ViewBuilder
{
	readonly View targetView;
	List<ViewControl> controls = [];

	public ViewBuilder(View targetView)
	{
		this.targetView = targetView;
	}

	public ViewBuilder AddControl(ViewControl control)
	{
		controls.Add(control);
		return this;
	}

	public List<ViewControl> Build()
	{
		targetView.Controls = controls;
		return controls;
	}
}
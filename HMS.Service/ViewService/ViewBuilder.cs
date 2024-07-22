using HMS.Service.ViewService.Controls;

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
		if (containers.TryPeek(out var container))
		{
			container.AddChild(control);
		}

		controls.Add(control);
		lastControl = control;
		return this;
	}

	public ViewBuilder Setup<T>(Action<T> controlAction) where T : ViewControl
	{
		if (lastControl != null)
		{
			controlAction((lastControl as T)
				?? throw new InvalidOperationException("Could not cast to specified view control type"));
		}
		return this;
	}

	public ViewBuilder Place<T>(ref T placement) where T : ViewControl
    {
		if (lastControl != null)
		{
			placement = (T)lastControl;
		}

		return this;
	}

	readonly Stack<ControlContainer> containers = [];
	public ViewBuilder StartContainer(string containerName)
	{
		var container = new ControlContainer(containerName);
		AddControl(container);
        containers.Push(container);
		return this;
	}

	public ViewBuilder EndContainer()
	{
		if (containers.Count > 0)
		{
			containers.Pop(); 
		}
		return this;
	}

	public List<ViewControl> Build()
	{
		targetView.Controls = controls;
		return controls;
	}
}
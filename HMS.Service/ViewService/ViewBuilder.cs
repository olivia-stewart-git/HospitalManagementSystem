using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService;

/// <summary>
/// Builder that allows for the construction of a view's visual controls
/// </summary>
public class ViewBuilder
{
	readonly View targetView;
	readonly List<ViewControl> controls = [];

	ViewControl? lastControl;

	public ViewBuilder(View targetView)
	{
		this.targetView = targetView;
	}

	/// <summary>
	/// Adds a control to root or any active container scopes.
	/// </summary>
	/// <param name="control"></param>
	/// <returns></returns>
	public ViewBuilder AddControl(ViewControl control)
	{
		if (containers.TryPeek(out var container))
		{
			container.AddChild(control);
		}
		else
		{
			controls.Add(control);
		}

		lastControl = control;
		return this;
	}
	
	/// <summary>
	/// Perform additional setup on the last added view element
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="controlAction"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public ViewBuilder Setup<T>(Action<T> controlAction) where T : ViewControl
	{
		if (lastControl != null)
		{
			controlAction((lastControl as T)
				?? throw new InvalidOperationException("Could not cast to specified view control type"));
		}
		return this;
	}

	/// <summary>
	/// Allow storage of control into a field on view class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="placement"></param>
	/// <returns></returns>
	public ViewBuilder Place<T>(ref T placement) where T : ViewControl
    {
		if (lastControl != null)
		{
			placement = (T)lastControl;
		}

		return this;
	}

	/// <summary>
	/// Disables the last created control
	/// </summary>
	/// <returns></returns>
	public ViewBuilder Disabled()
	{
		if (lastControl != null)
		{
			lastControl.Enabled = false;
		}
		return this;
    }

	readonly Stack<ControlContainer> containers = [];

	/// <summary>
	/// Create a container scope that will act as parent to any subsequent controls
	/// </summary>
	/// <param name="containerName"></param>
	/// <returns></returns>
	public ViewBuilder StartContainer(string containerName)
	{
		var container = new ControlContainer(containerName);
		AddControl(container);
        containers.Push(container);
		return this;
	}

	/// <summary>
	/// End an existing container scope
	/// </summary>
	/// <returns></returns>
	public ViewBuilder EndContainer()
	{
		if (containers.Count > 0)
		{
			containers.Pop(); 
		}
		return this;
	}

	/// <summary>
	/// Builds controls to be used on view.
	/// </summary>
	/// <returns></returns>
	public List<ViewControl> Build()
	{
		targetView.Controls = controls;
		return controls;
	}
}
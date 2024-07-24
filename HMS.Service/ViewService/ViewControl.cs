using System.Reflection;
using HMS.Common;

namespace HMS.Service.ViewService;

/// <summary>
/// Base class for controls that get rendered to the screen
/// </summary>
public abstract class ViewControl : IChangePropagator<ViewControl>, IPropertyBinding
{
	protected ViewControl(string name)
	{
		Name = name;
		OnChange += (_, _) => PropagateBindings();
	}

	/// <summary>
	/// Retrieves all children recursively
	/// </summary>
	/// <returns></returns>
	public IEnumerable<ViewControl> Recurse()
	{
		List<ViewControl> list = [this];
		foreach (var control in Children)
		{
			list.AddRange(control.Recurse());
		}
		return list;
	}

	List<ViewControl> children = [];
	public IReadOnlyList<ViewControl> Children
	{
		get => children;
		set => children = [..value];
	}

	public void RemoveChildren(IEnumerable<ViewControl> controls)
	{
		foreach (var viewControl in controls)
		{
			RemoveChild(viewControl);
		}
	}

	public void RemoveChild(ViewControl child)
	{
		children.Remove(child);
	}

	public void AddChild(ViewControl control)
	{
		children.Add(control);
		control.Parent = this;
	}

    public View? ParentView { get; set; }
	public ViewControl? Parent { get; private set; }
	public string Name { get; }
	public bool Focused { get; protected set; } = false;
	public int YPosition { get; set; } = 0;
	public bool RenderControlledByParent { get; set; }

	bool enabled = true;
    public bool Enabled
    {
	    get => Parent is not { Enabled: false } && enabled;
	    set
		{
			OnChange?.Invoke(this, this);
			enabled = value;
		}
    }

    public EventHandler<ViewControl> OnChange { get; set; }
	public void DoChange() => OnChange?.Invoke(this, this);

	readonly Dictionary<string, PropertyInfo> propertiesBindingCache = [];
	readonly Dictionary<PropertyInfo, List<Action<object>>> bindingTargets = [];

	//Updates bindings by invoking callback to relevant subscribers
	void PropagateBindings()
	{
		if (!Enabled) 
		{
			return;
		}
		foreach (var bindingTarget in bindingTargets)
		{
			var property = bindingTarget.Key;
			var propertyValue = property.GetValue(this);
			foreach (var action in bindingTarget.Value)
			{
				action(propertyValue);
			}
		}
	}

	//Implementation of binding
	public void BindProperty<T>(Action<T> factoryBinding, string propertyName)
	{
		BindProperty((obj) => factoryBinding((T)obj), propertyName);
	}

	//Implementation of binding
	//Maps a specific property on the view to its properties
    public void BindProperty(Action<object> factoryBinding, string propertyName)
	{
		if (!propertiesBindingCache.ContainsKey(propertyName))
		{
			var propertyTarget = GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);
			if (propertyTarget == null)
			{
				throw new InvalidOperationException($"Cannot find target member: {propertyName} for property biding");
			}
			propertiesBindingCache[propertyName] = propertyTarget;
		}

		var targetProperty = propertiesBindingCache[propertyName];
		if (bindingTargets.TryGetValue(targetProperty, out var bindingActions))
		{
			bindingActions.Add(factoryBinding);
		}
		else
		{
			bindingTargets[targetProperty] = [factoryBinding];
		}

		PropagateBindings();
    }

	//Core shared rendering functionality
    public List<RenderElement> Render()
    {
	    var baseElements = OnRender();
	    if (Focused)
	    {
		    foreach (var renderElement in baseElements)
		    {
			    renderElement.BackGroundColor = PageConstants.FocusedColor;
		    }
	    }
	    return baseElements;
    }

	//Override for implementation on variations
	protected abstract List<RenderElement> OnRender();
}
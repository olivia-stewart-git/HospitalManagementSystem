using System.Reflection;
using HMS.Common;

namespace HMS.Service.ViewService;

public abstract class ViewControl : IChangePropagator<ViewControl>
{
	protected ViewControl(string name)
	{
		Name = name;
		OnChange += (_, _) => PropagateBindings();
	}

    public View? Parent { get; set; }
	public string Name { get; }
	public bool Focused { get; protected set; } = false;
	public int YPosition { get; set; } = 0;

	bool enabled = true;
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
	public void DoChange() => OnChange?.Invoke(this, this);

	readonly Dictionary<string, PropertyInfo> propertiesBindingCache = [];
	readonly Dictionary<PropertyInfo, List<Action<object>>> bindingTargets = [];

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

	public void BindProperty<T>(Action<T> factoryBinding, string propertyName)
	{
		BindProperty((obj) => factoryBinding((T)obj), propertyName);
	}

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

	public abstract RenderElement Render();
}
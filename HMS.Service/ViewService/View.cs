using System.Text;

namespace HMS.Service.ViewService;

public abstract class View
{
	List<ViewControl> controls = [];
    readonly Dictionary<string, ViewControl> controlCache = [];

    public IReadOnlyList<ViewControl> Controls
	{
		get => controls;
		set
		{
			controls = [..value];
			RegenControlCache();
		}
	}

	public void AddControl(ViewControl control)
	{
		controls.Add(control);
		RegenControlCache();
	}

	void RegenControlCache()
	{
		foreach (var viewControl in controls)
		{
			var key = viewControl.GetType().Name + "_" + viewControl.Name;
			controlCache.TryAdd(key, viewControl);
		}
	}

	public abstract void BuildView(ViewBuilder viewBuilder);

	public T? Q<T>(string key) where T : ViewControl
	{
		var typeKey = typeof(T).Name + '_' + key;
		return controlCache.GetValueOrDefault(typeKey) as T;
	}

	public IList<RenderElement> Render()
	{
		return Controls.Where(x => x.Enabled).Select(x => x.Render()).ToList();
	}
	public virtual void OnBecomeActive()
	{
	}

    public virtual void OnUnload()
	{
	}
}
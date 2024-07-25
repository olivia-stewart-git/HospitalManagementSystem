using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService;

/// <summary>
/// Base class for all "Views" (Pages) that are shown in the application
/// </summary>
public abstract class View
{
	public EventHandler<ViewControl?> SelectionChanged { get; set; }

	public LinkedList<INavControl> NavControls => new (Controls.Where(x => x.Enabled).OfType<INavControl>());
	readonly Dictionary<string, ViewControl> controlCache = [];

    public ViewControl Root { get; } = new RootControl("root");

	LinkedListNode<INavControl>? selectedControlNode;

	INavControl? SelectedControl => selectedControlNode?.Value;

    public IEnumerable<ViewControl> Controls
	{
		get => Root.Recurse();
        set
		{
			Root.Children = [..value];
			RegenControlCache();
		}
	}

	//Adds a control
	public void AddControl(ViewControl control)
	{
		Root.AddChild(control);
		RegenControlCache();
	}

	//We keep a cache of controls mapped to their name
	void RegenControlCache()
	{
		NavControls.Clear();
		foreach (var viewControl in Controls)
		{
			viewControl.ParentView = this;
            if (viewControl is INavControl navControl && viewControl.Enabled)
			{
				NavControls.AddLast(navControl);
			}
			var key = viewControl.GetType().Name + "_" + viewControl.Name;
			controlCache.TryAdd(key, viewControl);
		}
	}

	/// <summary>
	/// Overridable method to access view builder to construct app page
	/// </summary>
	/// <param name="viewBuilder"></param>
	public abstract void BuildView(ViewBuilder viewBuilder);

	/// <summary>
	/// Query view for elements by name
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	public T Q<T>(string key) where T : ViewControl
	{
		var typeKey = typeof(T).Name + '_' + key;
		return controlCache.GetValueOrDefault(typeKey) as T 
			?? throw new ArgumentNullException(nameof(key));
	}

	//Renders elements by rendering each element, expect when they are disabled
	public IList<RenderElement> Render()
	{
		var renderOutput = new List<RenderElement>();
		var controlsToRender = Controls.Where(x => x.Enabled);
		var yPosition = 0;
		foreach (var control in controlsToRender)
		{
			if (control.RenderControlledByParent)
			{
				continue;
			}
			foreach (var baseRender in control.Render())
			{
				renderOutput.Add(baseRender);
				control.YPosition = yPosition;

				//For controlling cursor position, now obsolete
				var lineCount = baseRender.Contents.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length + 1;
				yPosition += lineCount;
			}
		}
		return renderOutput;
	}

#region View Events
//Overridable methods
	public virtual void OnBecomeActive()
	{
	}

    public virtual void OnUnload()
	{
	}

	public virtual void OnEscapePressed()
	{
	}
#endregion

	public void NavigateDown()
	{
		if (NavControls.Count == 0)
		{
			return;
		}
		if (selectedControlNode == null)
		{
			selectedControlNode = NavControls.First;
			SelectedControl?.NavigateEnter();
		}
		else
		{
			SelectedControl?.NavigateExit();
			selectedControlNode = selectedControlNode.Next ?? NavControls.First;
			SelectedControl?.NavigateEnter();
		}

		SelectionChanged?.Invoke(this, SelectedControl as ViewControl);
    }

	public void NavigateUp()
	{
		if (NavControls.Count == 0)
		{
			return;
		}
		if (selectedControlNode == null)
		{
			selectedControlNode = NavControls.Last;
			SelectedControl?.NavigateEnter();
		}
		else
		{
			SelectedControl?.NavigateExit();
			selectedControlNode = selectedControlNode.Previous ?? NavControls.Last;
			SelectedControl?.NavigateEnter();
		}

		SelectionChanged?.Invoke(this, SelectedControl as ViewControl);
    }
}
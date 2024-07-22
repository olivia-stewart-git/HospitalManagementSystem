using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService;

public abstract class View
{
	public EventHandler<ViewControl?> SelectionChanged { get; set; }

	readonly LinkedList<INavControl> navControls = [];
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

	public void AddControl(ViewControl control)
	{
		Root.AddChild(control);
		RegenControlCache();
	}

	void RegenControlCache()
	{
		navControls.Clear();
		foreach (var viewControl in Controls)
		{
			viewControl.ParentView = this;
            if (viewControl is INavControl navControl && viewControl.Enabled)
			{
				navControls.AddLast(navControl);
			}
			var key = viewControl.GetType().Name + "_" + viewControl.Name;
			controlCache.TryAdd(key, viewControl);
		}
	}

	public abstract void BuildView(ViewBuilder viewBuilder);

	public T Q<T>(string key) where T : ViewControl
	{
		var typeKey = typeof(T).Name + '_' + key;
		return controlCache.GetValueOrDefault(typeKey) as T 
			?? throw new ArgumentNullException(nameof(key));
	}

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
				if (control.Focused)
				{
					baseRender.BackGroundColor = ConsoleColor.Green;
				}

				renderOutput.Add(baseRender);
				control.YPosition = yPosition;

				var lineCount = baseRender.Contents.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length + 1;
				yPosition += lineCount;
			}
		}
		return renderOutput;
	}

#region View Events
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
		if (navControls.Count == 0)
		{
			return;
		}
		if (selectedControlNode == null)
		{
			selectedControlNode = navControls.First;
			SelectedControl?.NavigateEnter();
		}
		else
		{
			SelectedControl?.NavigateExit();
			selectedControlNode = selectedControlNode.Next ?? navControls.First;
			SelectedControl?.NavigateEnter();
		}

		SelectionChanged?.Invoke(this, SelectedControl as ViewControl);
    }

	public void NavigateUp()
	{
		if (navControls.Count == 0)
		{
			return;
		}
		if (selectedControlNode == null)
		{
			selectedControlNode = navControls.Last;
			SelectedControl?.NavigateEnter();
		}
		else
		{
			SelectedControl?.NavigateExit();
			selectedControlNode = selectedControlNode.Previous ?? navControls.Last;
			SelectedControl?.NavigateEnter();
		}

		SelectionChanged?.Invoke(this, SelectedControl as ViewControl);
    }
}
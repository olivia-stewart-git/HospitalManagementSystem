using System.Text;
using HMS.Common;

namespace HMS.Service.ViewService;

public abstract class View
{
	public EventHandler<ViewControl?> SelectionChanged { get; set; }

	List<ViewControl> controls = [];
	LinkedList<INavControl> navControls = [];
	LinkedListNode<INavControl>? selectedControlNode;

	INavControl? SelectedControl => selectedControlNode?.Value;

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
		navControls.Clear();
		foreach (var viewControl in controls)
		{
			if (viewControl is INavControl navControl)
			{
				navControls.AddLast(navControl);
			}
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
		var renderOutput = new List<RenderElement>();
		var controlsToRender = Controls.Where(x => x.Enabled);
		var yPosition = 0;
		foreach (var control in controlsToRender)
		{
			var baseRender = control.Render();
			if (control.Focused)
			{
				baseRender.BackGroundColor = ConsoleColor.Green;
			}
			renderOutput.Add(baseRender);
			control.YPosition = yPosition;

			var lineCount = baseRender.Contents.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length + 1;
			yPosition += lineCount;
		}
		return renderOutput;
	}

	public virtual void OnBecomeActive()
	{
	}

    public virtual void OnUnload()
	{
	}

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
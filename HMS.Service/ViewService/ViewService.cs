using HMS.Service.Interaction;

namespace HMS.Service.ViewService;

public class ViewService : IViewService, IDisposable
{
	readonly IViewWriter writer;
	readonly IServiceProvider serviceProvider;
	readonly IInputService inputService;

	readonly Stack<Type> viewHistory = [];
	readonly ICollection<IDisposable> disposables;

	public ViewService(IViewWriter writer, IServiceProvider serviceProvider, IInputService inputService)
	{
		this.writer = writer;
		this.serviceProvider = serviceProvider;
		this.inputService = inputService;
		disposables =
		[
			inputService.SubscribeToKeyAction(ConsoleKey.UpArrow, HandleUpArrow),
			inputService.SubscribeToKeyAction(ConsoleKey.DownArrow, HandleDownArrow),
			inputService.SubscribeToKeyAction(ConsoleKey.Escape, HandleEscapeKey)
        ];
	}

    #region InputHandling
    void HandleUpArrow()
    {
	    currentView?.NavigateUp();
    }

    void HandleDownArrow()
    {
	    currentView?.NavigateDown();
    }

    void HandleEscapeKey()
    {
	    currentView?.OnEscapePressed();
    }
    #endregion

    public View? CurrentView => currentView;
    View? currentView;

    public void LoadLastView()
    {
	    if (viewHistory.Count == 0)
	    {
		    throw new InvalidOperationException("Cannot load last view as no history recorded");
	    }
	    var lastView = viewHistory.Pop();
	    SwitchViewCore(lastView);
    }

	public T SwitchView<T>() where T : View
	{
		return (T)SwitchViewCore(typeof(T));
	}

	object SwitchViewCore(Type viewType)
	{
		if (CreateViewInstance(viewType) is not View instance)
		{
			throw new InvalidOperationException("View instance is not of View Type");
		}
		var builder = new ViewBuilder(instance);
		instance.BuildView(builder);

		builder.Build();

		if (currentView != null)
		{
			currentView.OnUnload();
			UnsubscribeFromView(currentView);
			UpdateHistory(currentView.GetType());
        }

		currentView = instance;
		SubscribeToView(currentView);

		currentView.NavigateDown();

		Redraw();
		currentView.OnBecomeActive();
		return instance;
    }

	void UpdateHistory(Type viewType)
	{
		viewHistory.Push(viewType);
    }

	object CreateViewInstance(Type viewType)
	{
		var constructorInfo = viewType.GetConstructors().First();
		var parameters = constructorInfo.GetParameters();
		var parameterValues = new object[parameters.Length];

		for (var i = 0; i < parameters.Length; i++)
		{
			var type = parameters[i].ParameterType;
			var value = serviceProvider.GetService(type);
			parameterValues[i] = value ?? throw new InvalidOperationException("Error occured resolving service for creating view");
		}

        var instance = Activator.CreateInstance(viewType, parameterValues);
		if (instance == null)
		{
			throw new InvalidOperationException("Error occured creating view instance");
		}
		return instance;
    }

	void UnsubscribeFromView(View view)
	{
		view.SelectionChanged -= HandleSelectionChanged;

        var elements = view.Controls;
		foreach (var viewControl in elements)
		{
			viewControl.OnChange -= HandleElementChange;
        }
	}

	void SubscribeToView(View view)
	{
		view.SelectionChanged += HandleSelectionChanged;
		var elements = view.Controls;
		foreach (var viewControl in elements)
		{
			viewControl.OnChange += HandleElementChange;
		}
    }

	void HandleSelectionChanged(object? sender, ViewControl? selectedControl)
	{
		inputService.ClearFill();
		if (selectedControl is IInputNode node)
		{
			inputService.FillInput(node);
        }
		Redraw();
    }

	void HandleElementChange(object? sender, ViewControl control)
	{
		Redraw();
	}

	public void Redraw()
	{
		if (CurrentView == null)
		{
			throw new InvalidOperationException("Cannot Redraw With no current view");
		}

		var value = CurrentView?.Render();
		writer.Clear();
		if (value != null)
		{
			foreach (var renderElement in value)
			{
				writer.WriteLine(renderElement);
			}
		}
	}

	public void Dispose()
	{
		foreach (var disposable in disposables)
		{
			disposable.Dispose();
		}
	}
}
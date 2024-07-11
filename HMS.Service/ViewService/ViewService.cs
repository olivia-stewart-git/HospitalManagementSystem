using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HMS.Service.Interaction;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService;

public class ViewService : IViewService, IDisposable
{
	readonly IViewWriter writer;
	readonly IServiceProvider serviceProvider;
	readonly IInputService inputService;

	ICollection<IDisposable> disposables;

	public ViewService(IViewWriter writer, IServiceProvider serviceProvider, IInputService inputService)
	{
		this.writer = writer;
		this.serviceProvider = serviceProvider;
		this.inputService = inputService;
		disposables =
		[
			inputService.SubscribeToKeyAction(ConsoleKey.UpArrow, HandleUpArrow),
			inputService.SubscribeToKeyAction(ConsoleKey.DownArrow, HandleDownArrow)
        ];
	}

	void HandleUpArrow()
	{
		currentView?.NavigateUp();
	}

	void HandleDownArrow()
	{
		currentView?.NavigateDown();
	}

	public View? CurrentView => currentView;

    View? currentView;

	public T SwitchView<T>() where T : View
	{
		var instance = CreateViewInstance<T>();
		var builder = new ViewBuilder(instance);
		instance.BuildView(builder);

		builder.Build();

		if (currentView != null)
		{
			currentView.OnUnload();
			UnsubscribeFromView(currentView);
        }

		currentView = instance;
		SubscribeToView(currentView);

		currentView.NavigateDown();

        Redraw();
        currentView.OnBecomeActive();
		return instance;
	}

	T CreateViewInstance<T>()
	{
		var viewType = typeof(T);

		var constructorInfo = viewType.GetConstructors().First();
		var parameters = constructorInfo.GetParameters();
		var parameterValues = new object[parameters.Length];

		for (int i = 0; i < parameters.Length; i++)
		{
			var type = parameters[i].ParameterType;
			var value = serviceProvider.GetService(type);
			parameterValues[i] = value ?? throw new InvalidOperationException("Error occured resolving service for creating view");
		}

        var instance = (T?)Activator.CreateInstance(viewType, parameterValues);
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
		if (selectedControl is IInputBlocker inputBlocker)
		{
			inputService.FillInput(inputBlocker);
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
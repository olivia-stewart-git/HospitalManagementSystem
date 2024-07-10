using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HMS.Service.ViewService;

public class ViewService : IViewService
{
	readonly IViewWriter writer;
	readonly IServiceProvider serviceProvider;

	public ViewService(IViewWriter writer, IServiceProvider serviceProvider)
	{
		this.writer = writer;
		this.serviceProvider = serviceProvider;
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
		var elements = view.Controls;
		foreach (var viewControl in elements)
		{
			viewControl.OnChange -= HandleElementChange;
        }
	}

	void SubscribeToView(View view)
	{
		var elements = view.Controls;
		foreach (var viewControl in elements)
		{
			viewControl.OnChange += HandleElementChange;
		}
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
}
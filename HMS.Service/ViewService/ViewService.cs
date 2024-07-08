using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Service.ViewService;

public class ViewService : IViewService
{
	readonly IViewWriter writer;

	public ViewService(IViewWriter writer)
	{
		this.writer = writer;
	}

	public View? CurrentView => currentView;

    View? currentView;

	public T SwitchView<T>() where T : View
	{
		var viewType = typeof(T);

        var instance = (T?)Activator.CreateInstance(viewType);
		if (instance == null)
		{
			throw new InvalidOperationException("Error occured creating view instance");
		}

		var builder = new ViewBuilder(instance);
		var methodName = nameof(instance.BuildView);

		viewType.GetMethod(methodName)?.Invoke(instance, [builder]);

		builder.Build();

		currentView?.OnUnload();

		currentView = instance;
		Redraw();
		return instance;
	}

	public void Redraw()
	{
		if (CurrentView == null)
		{
			throw new InvalidOperationException("Cannot Redraw With no current view");
		}

		var value = CurrentView?.Render() ?? string.Empty;
		writer.Clear();
		writer.Write(value);
		writer.WriteLine();
	}
}
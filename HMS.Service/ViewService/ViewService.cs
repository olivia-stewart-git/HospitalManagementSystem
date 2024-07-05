using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.ViewService;

public interface IViewService
{
}

public enum ViewTypes
{
	LoginMenu,
	PatientMenu,
}

public class ViewService : IViewService
{
}

public class View
{
	readonly ICollection<ViewControl> controls;

	public View(ICollection<ViewControl> controls)
	{
		this.controls = controls;
	}

	public string GetText()
	{
		return string.Empty;
	}

	public void Render()
	{
	}
}

public abstract class ViewControl
{
	public abstract string GetView();
}

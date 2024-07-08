using System.Text;

namespace HMS.Service.ViewService;

public abstract class View
{
	public List<ViewControl> Controls { get; set; } = [];

	public abstract void BuildView(ViewBuilder viewBuilder);

	public string Render()
	{
		var sb = new StringBuilder();
		foreach (var viewControl in Controls)
		{
			sb.AppendLine(viewControl.Render());
		}
		return sb.ToString().Trim(Environment.NewLine.ToCharArray());
	}

	public virtual void OnUnload()
	{
	}
}
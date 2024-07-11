namespace HMS.Service.ViewService.Controls;

public class NewLine : ViewControl
{
	public NewLine() : base(string.Empty)
	{
	}

	public override List<RenderElement> Render()
	{
		return [RenderElement.Default(System.Environment.NewLine)];
	}
}
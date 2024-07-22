namespace HMS.Service.ViewService.Controls;

public class NewLine : ViewControl
{
	public NewLine() : base(string.Empty)
	{
	}

	protected override List<RenderElement> OnRender()
	{
		return [RenderElement.Default(System.Environment.NewLine)];
	}
}
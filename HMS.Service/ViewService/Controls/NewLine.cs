namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Newline control
/// </summary>
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
namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Label that displays text
/// </summary>
public class Label : ViewControl
{
	string text;
	public string Text
	{
		get => text;
		set
		{
			text = value; 
			RegisterChanged();
		}
	}

	public ConsoleColor Color { get; set; } = PageConstants.DefaultColor;

	public Label(string text, string? name = null) : base(name ?? string.Empty)
    {
		this.Text = text;
	}

	protected override List<RenderElement> OnRender()
	{
		return [RenderElement.Colored(Text, Color)];
	}
}
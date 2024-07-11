namespace HMS.Service.ViewService;

public class RenderElement
{
	public string Contents { get; }
	public ConsoleColor Color { get; }
	public ConsoleColor BackGroundColor { get; set; }

	RenderElement(string contents, ConsoleColor color)
	{
		Contents = contents;
		Color = color;
	}

	public static RenderElement Default(string contents)
		=> new (contents, PageConstants.DefaultColor);

	public static RenderElement Colored(string contents, ConsoleColor color)
		=> new (contents, color);

	public static RenderElement Empty => Default(string.Empty);
}
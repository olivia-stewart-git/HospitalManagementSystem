namespace HMS.Service.ViewService;

public class RenderElement
{
	public string Contents { get; }
	public ConsoleColor Color { get; }
	public ConsoleColor BackGroundColor { get; set; }

	RenderElement(string contents, ConsoleColor color, ConsoleColor backGroundColor)
	{
		Contents = contents;
		Color = color;
		BackGroundColor = backGroundColor;
	}

	public static RenderElement Default(string contents)
		=> new (contents, PageConstants.DefaultColor, PageConstants.DefaultBackgroundColor);

	public static RenderElement Colored(string contents, ConsoleColor color)
		=> new(contents, color, PageConstants.DefaultBackgroundColor);

    public static RenderElement Colored(string contents, ConsoleColor color, ConsoleColor backgroundColor)
		=> new (contents, color, backgroundColor);

	public static RenderElement Empty => Default(string.Empty);
}
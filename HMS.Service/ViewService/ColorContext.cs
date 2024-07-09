namespace HMS.Service.ViewService;

public class ColorContext : IDisposable
{
	const ConsoleColor DefaultColor = ConsoleColor.Black;
	public ConsoleColor Color { get; }
	bool isForeground;

	ColorContext(ConsoleColor color)
	{
		this.Color = color;
	}

	public static ColorContext UseForeGround(ConsoleColor color)
	{
		var instance = new ColorContext(color);
		instance.isForeground = true;
		instance.SetColor(instance.Color);
		return instance;
	}

	public static ColorContext UseBackGround(ConsoleColor color)
	{
		var instance = new ColorContext(color);
		instance.isForeground = false;
		instance.SetColor(instance.Color);
		return instance;
	}

	void SetColor(ConsoleColor color)
	{
		if (isForeground)
		{
			Console.ForegroundColor = color; 
		}
		else
		{
			Console.BackgroundColor = color;
		}
	}

	public void Dispose()
	{
		Console.ResetColor();
	}
}
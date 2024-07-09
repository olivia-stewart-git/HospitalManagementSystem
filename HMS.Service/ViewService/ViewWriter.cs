namespace HMS.Service.ViewService;

public class ViewWriter : IViewWriter
{
	public void Clear()
	{
		Console.Clear();
	}

	public void Write(RenderElement renderElement)
	{
		using var colorContext = ColorContext.UseForeGround(renderElement.Color);
		Console.Write(renderElement.Contents);
	}

	public void WriteLine(RenderElement renderElement)
	{
		using var colorContext = ColorContext.UseForeGround(renderElement.Color);
        Console.WriteLine(renderElement.Contents);
	}

    public void Write(string value)
	{
		Console.Write(value);
	}

	public void WriteLine(string value)
	{
		Console.WriteLine(value);
	}

	public void WriteLine()
	{
		Console.WriteLine();
	}
}
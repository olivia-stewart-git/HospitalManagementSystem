﻿namespace HMS.Service.ViewService;

/// <summary>
/// Writes elements to the console.
/// If I had more time I would back elements by color, and write those in backs to avoid
/// slow flushing of console buffer.
/// </summary>
public class ViewWriter : IViewWriter
{
	public void Clear()
	{
		Console.Clear();
	}

	//we use class color context to make it easier to control console colors
	public void Write(RenderElement renderElement)
	{
		using var backGroundContext = ColorContext.UseBackGround(renderElement.BackGroundColor);
        using var colorContext = ColorContext.UseForeGround(renderElement.Color);
		Console.Write(renderElement.Contents);
	}

	public void WriteElement(RenderElement renderElement)
	{
		using var backGroundContext = ColorContext.UseBackGround(renderElement.BackGroundColor);
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
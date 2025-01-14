﻿namespace HMS.Service.ViewService.Controls;

/// <summary>
/// A box that displays output messages to user
/// </summary>
public class OutputBox : ViewControl
{
	public string Contents { get; private set; }
	public OutputState State { get; private set; }
	ConsoleColor outputColor = PageConstants.DefaultColor;

	public OutputBox(string contents, string? name = null) : base(name ?? string.Empty)
	{
		Contents = contents;
	}

	public void SetState(string newContents, OutputState state)
	{
		Contents = newContents;
		State = state;
		outputColor = state switch
		{
			OutputState.Info => ConsoleColor.Black,
			OutputState.Warn => ConsoleColor.Yellow,
			OutputState.Error => ConsoleColor.Red,
			OutputState.Success => ConsoleColor.DarkGreen,
			_ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
		};
		RegisterChanged();
	}

	public enum OutputState
	{
		Info,
		Warn,
		Error,
		Success
	}

	protected override List<RenderElement> OnRender()
	{
		return [RenderElement.Colored(Contents, outputColor)];
	}
}
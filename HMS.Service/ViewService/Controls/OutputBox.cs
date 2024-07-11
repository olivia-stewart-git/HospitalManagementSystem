namespace HMS.Service.ViewService.Controls;

public class OutputBox : ViewControl
{
	public string Contents { get; private set; }
	ConsoleColor outputColor = PageConstants.DefaultColor;

	public OutputBox(string contents, string? name = null) : base(name ?? string.Empty)
	{
		Contents = contents;
	}

	public void SetState(string newContents, OutputState state)
	{
		Contents = newContents;
		outputColor = state switch
		{
			OutputState.Info => ConsoleColor.Black,
			OutputState.Warn => ConsoleColor.Yellow,
			OutputState.Error => ConsoleColor.Red,
			OutputState.Success => ConsoleColor.DarkGreen,
			_ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
		};
		DoChange();
	}

	public enum OutputState
	{
		Info,
		Warn,
		Error,
		Success
	}

	public override List<RenderElement> Render()
	{
		return [RenderElement.Colored(Contents, outputColor)];
	}
}
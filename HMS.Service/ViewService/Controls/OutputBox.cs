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
		switch (state)
		{
			case OutputState.Info:
				outputColor = ConsoleColor.Black;
				break;
			case OutputState.Warn:
				outputColor = ConsoleColor.Yellow;
				break;
			case OutputState.Error:
				outputColor = ConsoleColor.Red;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}
		DoChange();
	}

	public enum OutputState
	{
		Info,
		Warn,
		Error,
	}

	public override RenderElement Render()
	{
		return RenderElement.Colored(Contents, outputColor);
	}
}
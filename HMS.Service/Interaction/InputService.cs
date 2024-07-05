namespace HMS.Service.Interaction;

public class InputService : IInputService
{
	public string ReadInput()
	{
		return ReadInput(string.Empty);
	}

	public string ReadInput(string prompt)
	{
		return Console.ReadLine() ?? string.Empty;
	}

	public char ReadChar()
	{
		return (char)Console.Read();
	}
}
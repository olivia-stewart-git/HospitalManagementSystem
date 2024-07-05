namespace HMS.Service.Interaction;

public class InputService : IInputService
{
	public int ReadIntegerInput()
	{
		var stringInput = ReadInput();
		if (int.TryParse(stringInput, out var intValue))
		{
			return intValue;
		}
		return -1;
	}

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
namespace HMS.Service.Interaction;

/// <summary>
/// Describes object that handles input form the keyboard for typing, such as an input field
/// </summary>
public interface IInputFiller : IInputNode
{
	(int x, int y) GetCursorPosition();
	void FillValue(char value);
}
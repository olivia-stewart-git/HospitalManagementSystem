namespace HMS.Service.Interaction;

public interface IInputFiller : IInputNode
{
	(int x, int y) GetCursorPosition();
	void FillValue(char value);
}
namespace HMS.Service.ViewService.Controls;

public interface IInputBlocker
{
	(int x, int y) GetCursorPosition();
	void FillValue(char value);
    void Pull();
}
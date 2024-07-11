namespace HMS.Service.Interaction;

public interface IInputSubscriber : IInputNode
{
	public void OnEnterInput();
	public void OnBackSpacePressed();
}
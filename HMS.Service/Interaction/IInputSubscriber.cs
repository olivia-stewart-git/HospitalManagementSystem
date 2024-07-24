namespace HMS.Service.Interaction;

/// <summary>
/// Subscribe to inputs in the system
/// </summary>
public interface IInputSubscriber : IInputNode
{
	public void OnEnterInput();
	public void OnBackSpacePressed();
}
using HMS.Service.Interaction;

namespace HMS.Service.ViewService.Controls;

public class InteractionOption : ViewControl, INavControl, IInputSubscriber
{
	public string Content { get; }
	public EventHandler Interacted { get; }

	public InteractionOption(string content, string name) : base(name)
	{
		Content = content;
	}

	public override RenderElement Render()
	{
		return RenderElement.Default(Content);
	}

	public bool IsHovered { get; set; }
	public void NavigateEnter()
	{
		IsHovered = true;
		Focused = true;
	}

	public void NavigateExit()
	{
		IsHovered = false;
		Focused = false;
	}

	public void OnEnterInput()
	{
		Interacted?.Invoke(this, EventArgs.Empty);
	}

	public void OnBackSpacePressed()
	{
	}
}
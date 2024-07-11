using HMS.Service.Interaction;

namespace HMS.Service.ViewService.Controls;

public class InteractionOption : ViewControl, INavControl, IInputSubscriber
{
	bool isInteractable = true;

	public bool IsInteractable
	{
		get => isInteractable;
		set
		{
			isInteractable = value;
			DoChange();
		}
	}

	public string Content { get; }
	public EventHandler Interacted { get; set; }

	public InteractionOption(string content, string name) : base(name)
	{
		Content = content;
	}

	public override List<RenderElement> Render()
	{
		return [RenderElement.Colored(
			Content,
			ConsoleColor.White,
			isInteractable
				? ConsoleColor.DarkBlue
				: ConsoleColor.DarkRed
			)];
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
		if (!IsInteractable)
		{
			return;
		}
		Interacted?.Invoke(this, EventArgs.Empty);
	}

	public void OnBackSpacePressed()
	{
	}
}
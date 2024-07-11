using HMS.Service.Interaction;

namespace HMS.Service.ViewService.Controls;

public class SelectionOption : ViewControl, INavControl, IInputSubscriber
{
	public delegate void SelectionAction(SelectionOption option);
	readonly SelectionAction selected;
	public string Name { get; }
	public int Index { get; set; }
	public int LeftPadding { get; set; } = 4;

	public SelectionOption(string name, SelectionAction selected) : base (name)
	{
		this.selected = selected;
		Name = name;
	}

	public override List<RenderElement> Render()
	{
		var value = string.Empty;
		for (int i = 0; i < LeftPadding; i++)
		{
			value += ' ';
		}

		value += $"{Index}. {Name}";
		return [RenderElement.Default(value)];
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
		if (Focused)
		{
			selected(this);
		}
	}

	public void OnBackSpacePressed()
	{
	}
}
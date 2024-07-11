namespace HMS.Service.ViewService.Controls;

public class SelectionOption
{
	public delegate void SelectionAction(SelectionOption option);
	readonly SelectionAction selected;
	public string Name { get; }

	public SelectionOption(string name, SelectionAction selected)
	{
		this.selected = selected;
		Name = name;
	}
}
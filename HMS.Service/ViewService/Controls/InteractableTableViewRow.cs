using HMS.Service.Interaction;

namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Singular interactable row in an interactable table view
/// </summary>
/// <typeparam name="T"></typeparam>
public class InteractableTableViewRow<T> : TableViewRow<T>, INavControl, IInputSubscriber
{
	readonly Action<InteractableTableViewRow<T>> selectionAction;

	public InteractableTableViewRow(
		string name, 
		IEnumerable<TableViewColumn<T>> tableColumns, 
		TableView<T> parent,
		T value,
		Action<InteractableTableViewRow<T>> selectionAction) : base(name, tableColumns, parent, value)
	{
		this.selectionAction = selectionAction;
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
		selectionAction(this);
	}

	public void OnBackSpacePressed()
	{
	}
}
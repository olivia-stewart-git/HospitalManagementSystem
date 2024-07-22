namespace HMS.Service.ViewService.Controls;

public class InteractableTableView<T> : TableView<T>
{
	public delegate void RowSelected(T rowValue);

	public event RowSelected Selected = value => {};

	public InteractableTableView(string name, IEnumerable<TableViewColumn<T>> tableColumns) : base(name, tableColumns)
	{
	}

	public InteractableTableView(string name, params TableViewColumn<T>[] tableColumns) : base(name, tableColumns)
	{
	}

	protected override TableViewRow<T> CreateRow(T value, int index)
		=> new InteractableTableViewRow<T>(typeof(T).Name + index, tableColumns, this, value, (row) => Selected.Invoke(row.Value));
}
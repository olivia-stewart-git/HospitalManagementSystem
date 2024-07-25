using HMS.Common;
using System.Linq.Expressions;

namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Provides a table of data mapped to a list of objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class TableView<T> : ViewControl
{
	protected readonly TableViewColumn<T>[] tableColumns;
	public int Padding { get; init; } = 150;
	public int MaxRows { get; init; } = 10;
	public bool CullPropertyPrefix { get; init; } = true;
	public int CalculatedWidth => PageConstants.PageWidth - Padding;

	public IEnumerable<T> Rows { get; private set; } = [];
	IEnumerable<TableViewRow<T>> renderRows = [];

	public TableView(string name, IEnumerable<TableViewColumn<T>> tableColumns) : base(name)
	{
		this.tableColumns = tableColumns.ToArray();
	}

    public TableView(string name, params TableViewColumn<T>[] tableColumns) : this(name, (IEnumerable<TableViewColumn<T>>) tableColumns)
	{
	}

	public void Bind(IChangePropagator<IEnumerable<T>> propagator)
	{
		propagator.OnChange += HandleChange;
	}

    void HandleChange(object? sender, IEnumerable<T> e)
    {
        Update(e);
    }

    public void Update(IEnumerable<T> updatedValues)
	{
		Rows = updatedValues;

		RemoveChildren(renderRows);

		var rowList = new List<TableViewRow<T>>();
		int index = 0;
		foreach (var row in Rows)
		{
			if (index >= MaxRows - 1)
			{
				break;
			}
			var instance = CreateRow(row, index);
			rowList.Add(instance);
			index++;
        }
		renderRows = rowList;
		DoChange();
    }

    TableViewRow<T> CreateRow(T value, int index)
    {
	    var row = CreateRowCore(value, index);
		AddChild(row);
		row.RenderControlledByParent = true;
	    return row;
    }

    protected virtual TableViewRow<T> CreateRowCore(T value, int index)
		=> new TableViewRow<T>(typeof(T).Name + index, tableColumns, this, value);

    protected override List<RenderElement> OnRender()
	{
		var header = GetHeaderValues();
		var headerRow = TableViewRow<T>.WriteRow(header, CalculatedWidth);

		List<RenderElement> elements =
		[
			RenderElement.Default(headerRow),
			RenderElement.Colored(TableViewRow<T>.GetBreakLine(CalculatedWidth, true), ConsoleColor.Blue)
		];

		foreach (var row in renderRows)
		{
			elements.AddRange(row.Render());
		}

		return elements;
	}


	List<string> GetHeaderValues()
	{
		var values = new List<string>();
		foreach (var column in tableColumns)
		{
			string columnName;
			if (column.OverrideName is not null)
			{
				columnName = column.OverrideName;
			}
			else
			{
				columnName = (column.ValueFunc.Body as MemberExpression)?.Member.Name ?? string.Empty;
                if (CullPropertyPrefix && columnName.Length > 3)
                {
	                columnName = columnName[4..];
                }
			}

			values.Add(columnName);

        }

		return values;
	}
}
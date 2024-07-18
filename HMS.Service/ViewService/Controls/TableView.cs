using System.Linq.Expressions;
using System.Text;
using HMS.Common;

namespace HMS.Service.ViewService.Controls;

public class TableView<T> : ViewControl
{
	readonly TableViewColumn<T>[] tableColumns;
	public int Padding { get; init; } = 150;
	public int MaxRows { get; init; } = 10;
	public bool CullPropertyPrefix { get; init; } = true;
	public int CalculatedWidth => PageConstants.PageWidth - Padding;

	IEnumerable<T> rows = [];
	IEnumerable<TableViewRow<T>> renderRows = [];

	public TableView(string name, IEnumerable<TableViewColumn<T>> tableColumns) : base(name)
	{
		this.tableColumns = tableColumns.ToArray();
	}

    public TableView(string name, params TableViewColumn<T>[] tableColumns) : base(name)
	{
		this.tableColumns = tableColumns;
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
		rows = updatedValues;
		renderRows = rows.Select((x, i) => new TableViewRow<T>(typeof(T).Name + i, tableColumns, this, x));
		DoChange();
    }

	public override List<RenderElement> Render()
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
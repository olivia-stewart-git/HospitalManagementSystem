using System.Linq.Expressions;
using System.Text;

namespace HMS.Service.ViewService.Controls;

public class TableView<T> : ViewControl
{
	readonly TableViewColumn<T>[] tableColumns;
	public int Padding { get; set; } = 200;
	public int CalculatedWidth => PageConstants.PageWidth - Padding;

	IReadOnlyList<T> values = [];

	public TableView(string name, params TableViewColumn<T>[] tableColumns) : base(name)
	{
		this.tableColumns = tableColumns;
	}

	public void Bind()
	{
	}

	public void Update(IReadOnlyList<T> updatedValues)
	{
		this.values = updatedValues;
	}

	public override RenderElement Render()
	{
		var sb = new StringBuilder();
		var header = GetHeaderValues();
		var headerRow = WriteRow(header);
		sb.AppendLine(headerRow);
		sb.AppendLine(GetBreakLine());

		foreach (var value in values)
		{
			var colValues = GetColumnValuesForRow(value);
			var rowString = WriteRow(colValues);
			sb.AppendLine(rowString);
		}

		return RenderElement.Default(sb.ToString());
	}

	List<string> GetColumnValuesForRow(T value)
	{
		var valuesList = new List<string>();
        foreach (var column in tableColumns)
		{
			var compiledExpression = column.ValueFunc.Compile();
			var compiledResult = compiledExpression(value);

			var stringVal = compiledResult.ToString();
			valuesList.Add(stringVal);
		}

		return valuesList;
	}

	string GetBreakLine()
	{
		var sb = new StringBuilder();
		for (int i = 0; i < CalculatedWidth; i++)
		{
			if (i % 2 == 0)
			{
				sb.Append('-');
			}
			else
			{
				sb.Append(' ');
			}
		}
		return sb.ToString();
    }

	string WriteRow(IList<string> values)
	{
		var sb = new StringBuilder();
		var itemWidth = (CalculatedWidth - values.Count - 1) / values.Count;

        for (var i = 0; i < values.Count; i++)
        {
	        sb.Append(values[i]);
	        var unitWidth = itemWidth - values[i].Length;
	        if (unitWidth < 0)
	        {
		        values[i] = values[i].Substring(0, itemWidth - 1);
		        unitWidth = 0;
	        }

            for (int j = 0; j < unitWidth; j++)
			{
				sb.Append(' ');
			}

			sb.Append('|');
        }
		return sb.ToString();
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
			}

			values.Add(columnName);

        }

		return values;
	}
}
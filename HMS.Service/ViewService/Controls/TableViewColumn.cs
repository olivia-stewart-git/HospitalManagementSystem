using System.Linq.Expressions;

namespace HMS.Service.ViewService.Controls;

public class TableViewColumn<T>
{
	public Expression<Func<T, object>> ValueFunc { get; }
	public int ColumnLength { get; }
	public string? OverrideName { get; }

	public TableViewColumn(Expression<Func<T, object>> valueFunc, int columnLength = 1, string? overrideName = null)
	{
		ValueFunc = valueFunc;
		ColumnLength = columnLength;
		OverrideName = overrideName;
	}
}
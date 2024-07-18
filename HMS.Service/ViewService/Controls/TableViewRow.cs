using System.Text;

namespace HMS.Service.ViewService.Controls;

public class TableViewRow<T> : ViewControl
{
	readonly TableViewColumn<T>[] tableColumns;
	readonly TableView<T> parent;
	public T Value { get; set; }

	public TableViewRow(string name, IEnumerable<TableViewColumn<T>> tableColumns, TableView<T> parent, T value) : base(name)
	{
		Value = value;
		this.tableColumns = tableColumns.ToArray();
		this.parent = parent;
	}

	public override List<RenderElement> Render()
	{
		var colValues = GetColumnValuesForRow(Value);
		var result = WriteRow(colValues, parent.CalculatedWidth);
		return [RenderElement.Default(result), RenderElement.Colored(GetBreakLine(parent.CalculatedWidth), ConsoleColor.Gray), ];
	}

	List<string> GetColumnValuesForRow(T value)
	{
		var valuesList = new List<string>();
		for (var index = 0; index < tableColumns.Length; index++)
		{
			var column = tableColumns[index];
			var compiledExpression = column.ValueFunc.Compile();
			string resultant;
			try
			{
				var compiledResult = compiledExpression(value);
				resultant = compiledResult.ToString();
			}
			catch
			{
				resultant = "Error";
			}

			valuesList.Add(resultant);
		}

		return valuesList;
	}

	public class WrappedText
	{
		public WrappedText(string text, int index)
		{
			Text = text;
			Index = index;
		}
		public string Text { get; set; }
		public int Index { get; set; }
	}

	public static string WriteRow(IList<string> values, int width)
	{
		var sb = new StringBuilder();
		WriteWrapped(sb, values.Select((x, i) => new WrappedText(x, i)).ToList(), values.Count, width);
		return sb.ToString();
	}

	public static string GetBreakLine(int width, bool isSolid = false)
	{
		var sb = new StringBuilder();
		for (var i = 0; i < width; i++)
		{
			if (i % 2 == 0 || isSolid)
			{
				sb.Append('┄');
			}
			else
			{
				sb.Append(' ');
			}
		}
		return sb.ToString();
	}

    static void WriteWrapped(StringBuilder sb, List<WrappedText> wrappedText, int count, int width)
	{
		var itemWidth = (width - count - 1) / count;
		var textQueue = new Queue<WrappedText>(wrappedText);

		var extraWrap = new List<WrappedText>();

		for (var i = 0; i < count; i++)
		{
			var item = textQueue.Peek();
			if (item.Index == i)
			{
				textQueue.Dequeue();
				var unitWidth = itemWidth - item.Text.Length;
				if (unitWidth < 0)
				{
					var spillover = item.Text.Substring(itemWidth);
					extraWrap.Add(new WrappedText(spillover, i));

					item.Text = item.Text.Substring(0, itemWidth);
					unitWidth = 0;
				}
				sb.Append(item.Text);
				WriteEmptyCell(sb, unitWidth);
			}
			else
			{
				WriteEmptyCell(sb, itemWidth);
			}

			if (i < count - 1)
			{
				sb.Append('|');
			}
		}

		if (extraWrap.Count > 0)
		{
			sb.AppendLine();
			WriteWrapped(sb, extraWrap, count, width);
		}
	}

	static void WriteEmptyCell(StringBuilder sb, int width)
	{
		for (int i = 0; i < width; i++)
		{
			sb.Append(' ');
		}
	}
}
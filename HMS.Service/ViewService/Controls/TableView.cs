﻿using HMS.Common;
using System.Linq.Expressions;
using HMS.Service.Interaction;

namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Provides a table of data mapped to a list of objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class TableView<T> : ViewControl, IAddInputSubscriber
{
	protected readonly TableViewColumn<T>[] tableColumns;
	public int Padding { get; init; } = 150;
	public int MaxRows { get; init; } = 10;
	public bool CullPropertyPrefix { get; init; } = true;
	public int CalculatedWidth => PageConstants.PageWidth - Padding;

	public int ChunkIndex { get; private set; }
	public int MaxChunks { get; private set; } = 10;

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

	//regen rows and their associated values
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
		RegisterChanged();
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

		for (var index = 0; index < tableColumns.Length; index++)
		{
			var column = tableColumns[index];
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

			if (index == tableColumns.Length - 1)
			{
				var dataCap = MaxChunks > 0 ? $"{ChunkIndex}/{MaxChunks}" : string.Empty;
                columnName += "\t" + dataCap;
			}

			values.Add(columnName);
		}

		return values;
	}


	//Not implemented didnt have time
	public void OnRightArrow()
	{
		ChunkIndex++;
		if (ChunkIndex > MaxChunks)
		{
			ChunkIndex = 0;
		}

		RegisterChanged();
    }

	public void OnLeftArrow()
	{
		ChunkIndex--;
        if (ChunkIndex < 0)
        {
	        ChunkIndex = MaxChunks;
        }

		RegisterChanged();
	}
}
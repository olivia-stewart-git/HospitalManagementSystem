using System.Collections.Generic;
using System.Text;

namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Provides a list of selectable objects
/// </summary>
public class OptionsList : ViewControl
{
	public string Header { get; }
	public SelectionOption[] Options { get; }

	public OptionsList(string name, string header, params SelectionOption[] options) : this (name, header, (IEnumerable<SelectionOption>)options)
	{
	}

	public OptionsList(string name, string header, IEnumerable<SelectionOption> options) : base(name)
	{
		Header = header;
		Options = options.ToArray();
		Children = options.Cast<ViewControl>().ToList();
	}


    protected override List<RenderElement> OnRender()
	{
		List<RenderElement> elements = [RenderElement.Default(Header)];
		int count = 0;
		foreach (var selectionOption in Options)
		{
			selectionOption.Index = count++;
		}

		return elements;
	}

	public SelectionOption Get(int index)
	{
		return Options[index];
	}
}
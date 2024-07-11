using System.Text;

namespace HMS.Service.ViewService.Controls;

public class OptionsList : ViewControl
{
	public SelectionOption[] Options { get; }

	public OptionsList(string name, params SelectionOption[] options) : base(name)
	{
		Options = options;
	}

	public override RenderElement Render()
	{
		var sb = new StringBuilder();
		for (var index = 0; index < Options.Length; index++)
		{
			var option = Options[index];
			sb.AppendLine($"{index}. {option.Name}");
		}

		return RenderElement.Default(sb.ToString());
	}

	public SelectionOption Get(int index)
	{
		return Options[index];
	}
}
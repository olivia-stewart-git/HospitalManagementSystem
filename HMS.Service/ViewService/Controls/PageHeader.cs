using System.Text;

namespace HMS.Service.ViewService.Controls;

public class PageHeader : ViewControl
{
	public int Padding { get; set; } = 250;
	public int CalculatedWidth  => PageConstants.PageWidth - Padding;
    public string Title { get; }
	public string Subtitle { get; }

	const char BorderTop = '─';
    const char BorderTopRight = '┑';
    const char BorderTopLeft = '┌';
    const char BorderBottomLeft = '└';
    const char BorderBottomRight = '┙';
    const char BorderSide =  '│';

	public PageHeader(string title, string subtitle, string? name = null) : base (name ?? string.Empty)
	{
		this.Title = title;
		this.Subtitle = subtitle;
	}

	public override List<RenderElement> Render()
	{
		var width = CalculatedWidth;
        var sb = new StringBuilder();

        var headerLine = GetHeaderLine(width, BorderTopLeft, BorderTopRight);
        var bottomLine = GetHeaderLine(width, BorderBottomLeft, BorderBottomRight);
        var emptyLine = GetEmptyLine(width);
		var mainTitleLine = GetLine(Title, width);
		var dottedLine = GetDottedLine(width);
		var subtitleLine = GetLine(Subtitle, width);


        sb.AppendLine(headerLine)
	        .AppendLine(emptyLine)
            .AppendLine(mainTitleLine)
			.AppendLine(emptyLine)
			.AppendLine(dottedLine)
			.AppendLine(emptyLine)
			.AppendLine(subtitleLine)
			.AppendLine(emptyLine)
			.AppendLine(bottomLine);

		return [RenderElement.Default(sb.ToString())];
	}

	string GetHeaderLine(int headingWidth, char leftChar, char rightChar)
	{
		var sb = new StringBuilder();
		sb.Append(leftChar);
		for (int i = 0; i < headingWidth - 2; i++)
		{
			sb.Append(BorderTop); 
		}
		sb.Append(rightChar);
        return sb.ToString();
	}

    string GetEmptyLine(int width)
	{
        var sb = new StringBuilder();
        sb.Append(BorderSide);
        for (int i = 0; i < width - 2; i++)
		{
			sb.Append(' ');
		}
		sb.Append(BorderSide);
		return sb.ToString();
    }

	string GetLine(string contents, int headerWidth)
	{
		var sb = new StringBuilder();
		var headerCentre = headerWidth / 2 - 1;
		var contentHalf = contents.Length / 2 - 1;
		var contentBuffer = headerCentre - contentHalf;
		var remaining = headerWidth - contentBuffer - contents.Length - 2;

		sb.Append(BorderSide);
        for (int i = 0; i < contentBuffer; i++)
        {
	        sb.Append(' ');
        }

        sb.Append(contents);

        for (int i = 0; i < remaining; i++)
        {
	        sb.Append(' ');
        }

        sb.Append(BorderSide);
        return sb.ToString();
	}

	string GetDottedLine(int headerWidth)
	{
		var line = string.Empty;
		line += BorderSide;
        headerWidth -= 2;
		for (var i = 0; i < headerWidth; i++)
		{
			line += i % 2 == 0 ? BorderTop : ' ';
		}

		line += BorderSide;

		return line;
	}
}
﻿namespace HMS.Service.ViewService.Controls;

public class InputField : ViewControl, INavControl, IInputBlocker
{
	string contents = string.Empty;
	public string Prompt { get; }
	public int MaxLength { get; init; } = 15;
	public bool ObscureContent { get; init; } = false;

	public string Contents
	{
		get => contents;
		set
		{
			contents = value;
			DoChange();
		}
	}

	public string DisplayedContent
	{
		get
		{
			if (ObscureContent)
			{
				var obscuredContent = string.Empty;
				for (int i = 0; i < contents.Length; i++)
				{
					obscuredContent += '*';
				}

				return obscuredContent;
			}
			return contents;
		}
	}

	public InputField(string prompt, string name) : base(name)
	{
		Prompt = prompt;
	}

	public override RenderElement Render()
	{
		return RenderElement.Default(Prompt + DisplayedContent);
	}

	int CursorOffset => Prompt.Length + Contents.Length;

	public bool IsHovered { get; set; }
	public void NavigateEnter()
	{
		IsHovered = true;
		Focused = true;
	}

	public void NavigateExit()
	{
		IsHovered = false;
		Focused = false;
    }

	public (int x, int y) GetCursorPosition()
	{
		return (CursorOffset, YPosition);
	}

	public void FillValue(char value)
	{
		if (Contents.Length >= MaxLength)
		{
			return;
		}
		Contents += value;
	}

	public void Pull()
	{
		if (contents.Length <= 1)
		{
			Contents = string.Empty;
			return;
		}

		Contents = Contents.Remove(Contents.Length - 1);
	}
}
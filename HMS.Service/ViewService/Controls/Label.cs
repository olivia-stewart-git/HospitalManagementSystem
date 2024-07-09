﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.ViewService.Controls;

public class Label : ViewControl
{
	readonly string text;

	public Label(string text, string? name = null) : base(name ?? string.Empty)
    {
		this.text = text;
	}

	public override RenderElement Render()
	{
		return RenderElement.Default(text);
	}
}
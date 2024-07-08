using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.ViewService.Controls;

public class Label : ViewControl
{
	readonly string text;

	public Label(string text)
	{
		this.text = text;
	}

	public override string Render()
	{
		return text;
	}
}
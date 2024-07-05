using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.Interaction;

public interface IInputService
{
	public string ReadInput();
    public string ReadInput(string prompt);
	public char ReadChar();
}
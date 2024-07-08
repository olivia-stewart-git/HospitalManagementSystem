using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Service.TestUtilities;

public class ConsoleCapture : IDisposable
{
    readonly StringWriter sr;

	public ConsoleCapture()
	{
		sr = new StringWriter();
		Console.SetOut(sr);
    }

	public override string ToString()
	{
		return sr.ToString();
	}

	public void Dispose()
	{
		sr.Dispose();
		//Reset Console Out
		Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
}
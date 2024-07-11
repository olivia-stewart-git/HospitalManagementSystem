using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.Interaction;

public interface IInputService
{
	IDisposable SubscribeToCharacterAction(Action<char> characterAction);
	IDisposable SubscribeToKeyAction(ConsoleKey key, Action keyAction);
	void FillInput<T>(T inputNode) where T : IInputNode;
	void ClearFill();
}
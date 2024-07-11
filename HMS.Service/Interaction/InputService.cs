using HMS.Common;
using HMS.Service.ViewService;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.Interaction;

public class InputService : IInputService
{
	public InputService()
	{
		DeployThread();
	}

	void DeployThread()
	{
		Task.Run(ThreadAction);
	}

	List<Action<char>> characterActionSubscribers = [];

	public IDisposable SubscribeToCharacterAction(Action<char> characterAction)
	{
		characterActionSubscribers.Add(characterAction);
        return new DisposableAction(() => characterActionSubscribers.Remove(characterAction));
	}

	Dictionary<ConsoleKey, List<Action>?> keyActionMap = [];
	public IDisposable SubscribeToKeyAction(ConsoleKey key, Action keyAction)
	{
		if (keyActionMap.TryGetValue(key, out var actionList))
		{
			actionList ??= [];
			actionList.Add(keyAction);
		}
		else
		{
			keyActionMap[key] = [keyAction];
		}

		return new DisposableAction(() =>
		{
			if (keyActionMap.TryGetValue(key, out var disposeActionList))
			{
				disposeActionList ??= [];
				disposeActionList.Remove(keyAction);
			}
        });
	}

	readonly ConsoleKey[] specialKeys = [ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.Escape, ConsoleKey.UpArrow, ConsoleKey.DownArrow];

	void ThreadAction()
	{
		while (true)
		{
			var keyValue = Console.ReadKey(true);
			if (specialKeys.Contains(keyValue.Key))
			{
				if (keyValue.Key == ConsoleKey.Backspace)
				{
					BackSpaceForFill();
				} 
				else if (keyValue.Key == ConsoleKey.Enter)
				{
					EnterForFill();
				}
				SpecialKeyAction(keyValue);
			}
			else
			{
				OnCharacterAction(keyValue.KeyChar);
			}
		}
	}

	void OnCharacterAction(char action)
	{
		UpdateFill(action);
        foreach (var characterAction in characterActionSubscribers)
		{
			characterAction?.Invoke(action);
		}
	}

	void SpecialKeyAction(ConsoleKeyInfo keyInfo)
	{
		if (keyActionMap.TryGetValue(keyInfo.Key, out var actionSubscribers))
		{
			foreach (var actionSubscriber in actionSubscribers)
			{
				actionSubscriber?.Invoke();
			}
		}
	}

	IInputNode? currentInputTarget;

	public void FillInput<T>(T inputNode) where T : IInputNode
	{
		if (inputNode is IInputFiller filler)
		{
			FillInput(filler);
		}
	}
	void FillInput(IInputFiller inputFiller)
	{
		var cursorPosition = inputFiller.GetCursorPosition();
		Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
	}

    void UpdateFill(char value)
	{
		if (currentInputTarget is IInputFiller inputFiller)
		{
			inputFiller.FillValue(value);
			var cursorPosition = inputFiller.GetCursorPosition();
			Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
		}
	}

	void BackSpaceForFill()
	{
		if (currentInputTarget is IInputSubscriber inputSubscriber)
		{
			inputSubscriber.OnBackSpacePressed();
		}
	}

	void EnterForFill()
	{
		if (currentInputTarget is IInputSubscriber inputSubscriber)
		{
			inputSubscriber.OnEnterInput();
		}
    }

	public void ClearFill()
	{
		currentInputTarget = null;
	}
}
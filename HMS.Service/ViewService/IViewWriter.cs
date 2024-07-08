﻿namespace HMS.Service.ViewService;

public interface IViewWriter
{
	public void Clear();
	public void Write(string value);
	public void WriteLine(string value);
	public void WriteLine();
}
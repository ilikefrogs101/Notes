using Godot;
using System;

public static class Utils
{
	public static float RoundToNearestMultiple(float value, float multiple)
	{
		return Mathf.Round(value / multiple) * multiple;
	}
}
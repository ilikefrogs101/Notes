using Godot;
using System;
using System.Security.Cryptography;

public partial class GridGenerator : Control
{
    [Export] int cellSize = 50;
	public override void _Draw()
	{
		Vector2 gridSize = GetViewportRect().Size * 1.5f;
		
        for (int x = (int)-gridSize.X; x < gridSize.X; ++x)
        {
            DrawLine( 
				new Vector2(x * cellSize, -gridSize.X), 
				new Vector2(x * cellSize, gridSize.X * cellSize), 
				new Color(1, 1, 1)
			);
        }
        for (int y = (int)-gridSize.Y; y < gridSize.Y; ++y)
        {
            DrawLine(
				new Vector2(-gridSize.Y, y * cellSize), 
				new Vector2(gridSize.Y * cellSize, y * cellSize), 
				new Color(1, 1, 1)
			);
        }
	}

    public override void _PhysicsProcess(double delta)
    {
		Vector2 _GlobalPosition = MainCamera._camera.Offset + GetViewport().GetVisibleRect().Size / 2;
		_GlobalPosition.X = Utils.RoundToNearestMultiple(_GlobalPosition.X, cellSize);
		_GlobalPosition.Y = Utils.RoundToNearestMultiple(_GlobalPosition.Y, cellSize);
		GlobalPosition = _GlobalPosition;
    }
}
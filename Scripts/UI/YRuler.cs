using Godot;
using System;
using System.Security.Cryptography;

public partial class YRuler : Control
{
    [Export] int cellSize = 50;
	public override void _Draw()
	{
		Vector2 gridSize = GetViewportRect().Size * 1.5f;

		Vector2 _GlobalPosition = MainCamera._camera.Offset + GetViewport().GetVisibleRect().Size / 2;
		_GlobalPosition.Y = Utils.RoundToNearestMultiple(_GlobalPosition.Y, cellSize);
		GlobalPosition = _GlobalPosition;
        for (int y = (int)-gridSize.Y; y < gridSize.Y; ++y)
        {
            DrawLine( 
				new Vector2(315, y * 50), 
				new Vector2(340, y * 50), 
				new Color(0, 0, 0),
				5
			);
        }
	}

    public override void _PhysicsProcess(double delta)
    {
		QueueRedraw();
    }
}
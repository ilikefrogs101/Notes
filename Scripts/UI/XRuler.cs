using Godot;
using System;
using System.Security.Cryptography;

public partial class XRuler : Control
{
    [Export] int cellSize = 50;
	bool test = true;
	public override void _Draw()
	{
		Vector2 gridSize = GetViewportRect().Size * 1.5f;

		Vector2 _GlobalPosition = MainCamera._camera.Offset + GetViewport().GetVisibleRect().Size / 2;
		_GlobalPosition.X = Utils.RoundToNearestMultiple(_GlobalPosition.X, cellSize);
		GlobalPosition = _GlobalPosition;
        for (int x = (int)-gridSize.X; x < gridSize.X; ++x)
        {
            DrawLine( 
				new Vector2(x * 50, 345), 
				new Vector2(x * 50, 375), 
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
using Godot;
using System;

public partial class MainCamera : Camera2D
{
	public static MainCamera _camera;

	[ExportGroup("Movement")]
	[Export] float cameraSpeed;
	
	[ExportGroup("UI")]
	[Export] Label X;
	[Export] Label Y;

    public override void _Ready()
    {
        _camera = this;
    }
    public override void _PhysicsProcess(double delta)
    {
		Move();
		UI();
    }

	void Move()
	{
		Vector2 input = Vector2.Zero;
        if(Input.IsActionPressed("move_forward"))
		{
			input += Vector2.Up;
		}
		if(Input.IsActionPressed("move_back"))
		{
			input += Vector2.Down;
		}
		if(Input.IsActionPressed("move_left"))
		{
			input += Vector2.Left;
		}
		if(Input.IsActionPressed("move_right"))
		{
			input += Vector2.Right;
		}

		Offset += input.Normalized() * cameraSpeed;
	}

	void UI()
	{
		X.Text = Mathf.Round(MainCamera._camera.Offset.X / 50).ToString();
		Y.Text = (Mathf.Round(MainCamera._camera.Offset.Y / 50) * -1).ToString();
	}
}

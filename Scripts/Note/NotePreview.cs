using Godot;
using System;
using System.Collections;
using ilikefrogs101.Notes.Manager;

namespace ilikefrogs101.Notes.NoteComponents
{
	public partial class NotePreview : Control
	{
		[Export] Area2D mouseDetector;

		[ExportGroup("Editable Components")]
		[Export] Label text;
		[Export] Button deleteButton;

		[ExportGroup("Visuals")]
		[Export] ColorRect background;
		[Export] public Line2D connection;

		// Data
		Vector2 CursorOffset = new(60, 50);
		public Note data;
		bool mouseOver;
		bool dragging = false;

		public override void _Ready()
		{
			text.Text = data.Contents;

			deleteButton.Pressed += DeleteNote;
			mouseDetector.MouseEntered += PreviewEntered;
			mouseDetector.MouseExited += PreviewExited;

			data.OnContentsChanged += ContentsChanged;
			data.OnTitleChanged += ContentsChanged;
			data.OnColourChanged += ColourChanged;

			background.Color = data.Colour;
			text.Text = $"{data.Title}\n \n{data.Contents}";
		}
		public override void _Process(double delta)
		{
			// Dragging
			if(mouseOver && Input.IsActionJustPressed("move_note"))
				dragging = true;
			else if(mouseOver && dragging && Input.IsActionJustReleased("move_note"))
				dragging = false;

			if(dragging)
				GlobalPosition = GetGlobalMousePosition() - CursorOffset;
			
			// Clamp position inside the window
			GlobalPosition = GlobalPosition.Clamp(
						Vector2.Zero, 
						new Vector2(GetViewportRect().Size.X - (Size.X * 4f), GetViewportRect().Size.Y - (Size.Y * 3f)));

			if(!mouseOver) { return; }
			// Connections		
			if(Input.IsActionJustPressed("select_note") && !dragging) {
				NoteManager.Instance.CreateConnection(data);
				return;
			}

			// Interacting
			if(Input.IsActionJustPressed("open_note") && !dragging)
				if(!data.WindowVisible)
					data.window.Show();
		}

		void DeleteNote()
		{
			QueueFree();

			data.CallNoteDelete();
		}
		void PreviewEntered()
		{
			mouseOver = true;
		}	
		void PreviewExited()
		{
			mouseOver = false;
		}

		void ContentsChanged()
		{
			text.Text = $"{data.Title}\n \n{data.Contents}";
		}
		void ColourChanged()
		{
			background.Color = data.Colour;
		}
	}
}
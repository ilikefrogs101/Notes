using Godot;
using System;
using System.Collections;
using ilikefrogs101.Notes.Manager;
using ilikefrogs101.Notes;

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

		// Data
		Vector2 CursorOffset = new(60, 50);
		public Note data;
		bool mouseOverPreview;

		public override void _Ready()
		{
			text.Text = data.Contents;

			deleteButton.Pressed += DeleteNote;
			mouseDetector.MouseEntered += () => mouseOverPreview = true;
			mouseDetector.MouseExited += () => mouseOverPreview = false;

			data.OnContentsChanged += ContentsChanged;
			data.OnTitleChanged += ContentsChanged;
			data.OnColourChanged += ColourChanged;

			background.Color = data.Colour;
			text.Text = $"{data.Title}\n \n{data.Contents}";
		}
        public override void _ExitTree()
        {
            deleteButton.Pressed -= DeleteNote;
			mouseDetector.MouseEntered -= () => mouseOverPreview = true;
			mouseDetector.MouseExited -= () => mouseOverPreview = false;

			data.OnContentsChanged -= ContentsChanged;
			data.OnTitleChanged -= ContentsChanged;
			data.OnColourChanged -= ColourChanged;
        }
        
		public override void _Process(double delta)
		{
			// Clamp position inside the window
			GlobalPosition = GlobalPosition.Clamp(
				Vector2.Zero, 
				new Vector2(
					GetViewportRect().Size.X - (Size.X * 4f), 
					GetViewportRect().Size.Y - (Size.Y * 3f)
				)
			);
			
			if(MoveNote()) return;
			if(ConnectNote()) return;
			if(OpenNote()) return;
		}

		bool beingMoved = false;
		bool MoveNote()
		{
			if(Input.IsActionJustPressed("move_note") && mouseOverPreview)
				beingMoved = true;
			else if(Input.IsActionJustReleased("move_note"))
				beingMoved = false;
			
			if(beingMoved)
			{
				GlobalPosition = GetGlobalMousePosition() - CursorOffset;
				return true;
			}

			return false;
		}
		bool ConnectNote()
		{
			if(Input.IsActionJustPressed("connect_note") && mouseOverPreview)
			{
				NoteManager.Instance.CreateConnection(data);
				return true;
			}

			return false;
		}
		bool OpenNote()
		{
			if(Input.IsActionJustPressed("open_note") && !data.WindowVisible && mouseOverPreview)
			{
				data.window.Show();
				return true;
			}
			return false;
		}

		void DeleteNote()
		{
			QueueFree();

			data.CallNoteDelete();
		}
		void PreviewEntered()
		{
			mouseOverPreview = true;
		}	
		void PreviewExited()
		{
			mouseOverPreview = false;
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
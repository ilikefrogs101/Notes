using Godot;
using System;
using System.Collections;
using ilikefrogs101.Notes.Manager;
using ilikefrogs101.Notes;
using System.Collections.Generic;

namespace ilikefrogs101.Notes.NoteComponents
{
	public partial class NotePreview : Control
	{
		[Export] Area2D mouseDetector;

		[ExportGroup("Settings")]
		[Export] float repelForce = 7.5f;

		[ExportGroup("Editable Components")]
		[Export] Label text;
		[Export] Button deleteButton;

		[ExportGroup("Visuals")]
		[Export] ColorRect background;

		// Data
		Vector2 CursorOffset = new(40, 40);
		public Note data;
		private List<Control> collidingNotes = new();
		bool mouseOverPreview;
		
		public override void _Ready()
		{
			text.Text = data.Contents;

			deleteButton.Pressed += DeleteNote;
			mouseDetector.MouseEntered += () => mouseOverPreview = true;
			mouseDetector.MouseExited += () => mouseOverPreview = false;

			mouseDetector.AreaEntered += OnCollisionStart;
			mouseDetector.AreaExited += OnCollisionEnd;
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
			if(MoveNote()) return;
			if(ConnectNote()) return;
			if(OpenNote()) return;
		}
        public override void _PhysicsProcess(double delta)
        {
            // Handle collisions so notes can't overlap
			if(collidingNotes.Count > 0)
			{
				foreach(Control collidingNote in collidingNotes)
				{
					if(collidingNote == null) { collidingNotes.Remove(collidingNote); continue; }


					GlobalPosition = GlobalPosition.Lerp(
						GlobalPosition - (collidingNote.GlobalPosition - GlobalPosition).Normalized() * repelForce, 
						0.75f
					);
					collidingNote.GlobalPosition = collidingNote.GlobalPosition.Lerp(
						collidingNote.GlobalPosition - (GlobalPosition - collidingNote.GlobalPosition).Normalized() * repelForce, 
						0.75f
					);
				}
			}
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
			}

			return beingMoved;
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

		void OnCollisionStart(Area2D area)
		{
			collidingNotes.Add(area.GetParent<Control>());
		}
		void OnCollisionEnd(Area2D area)
		{
			collidingNotes.Remove(area.GetParent<Control>());
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
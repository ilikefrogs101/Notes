using Godot;
using System;
using ilikefrogs101.Notes.Manager;

namespace ilikefrogs101.Notes.UI
{
	public partial class NoteUI : Node
	{
		[ExportGroup("UI")]
		[Export] Button newNoteButton;

		public override void _Ready()
		{
			newNoteButton.Pressed += CreateNote;
		}

		/// <summary>
		/// Creates a new note
		/// </summary>
		public void CreateNote()
		{
			NoteManager.Instance.CreateNote();
		}
	}
}
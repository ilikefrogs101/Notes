using Godot;
using System;

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

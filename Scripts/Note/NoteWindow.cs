using Godot;
using System;
using System.Collections;

public partial class NoteWindow : Window
{
	[ExportGroup("Editable Components")]
	[Export] LineEdit titleEntry;
	[Export] CodeEdit text;
	[Export] ColorPickerButton colourButton;

	[ExportGroup("Visuals")]
	[Export] ColorRect background;

	// Data
	public NoteData data;

    public override void _Ready()
    {
        CloseRequested += CloseWindow;

		titleEntry.TextChanged += TitleChanged;
		colourButton.ColorChanged += ColourChanged;
        text.TextChanged += ContentsChanged;

		titleEntry.Text = data.Title;
		text.Text = data.Contents;
		colourButton.Color = data.Colour;

		Title = data.Title;
		background.Color = data.Colour;

		data.OnNoteDelete += DeleteNote;
		data.WindowVisible = true;
    }

	void TitleChanged(string newTitle)
	{
		Title = newTitle;
		data.Title = newTitle;

		data.CallOnTitle();
	}
	void ColourChanged(Color newColor)
	{
		background.Color = newColor;
		data.Colour = newColor;
		
		data.CallOnColour();
	}
	void ContentsChanged()
	{
		data.Contents = text.Text;

		data.CallOnContents();
	}

	void CloseWindow()
	{
		data.WindowPosition = Position;
		data.WindowVisible = false;
		Hide();
	}
	void DeleteNote()
	{
		data.WindowVisible = false;
		QueueFree();
		data.Dispose();
	}
}

using Godot;
using System;
using ilikefrogs101.Notes.Manager;

namespace ilikefrogs101.Notes.NoteComponents
{
	public partial class Note : Resource
	{
		public string ID;
		public string Title;
		public string Contents;
		public Color Colour;

		public NoteWindow window;
		public NotePreview preview;

		public bool WindowVisible;
		public Vector2 WindowPosition;

		/// <summary>
		/// Creates a note based on pre existing data
		/// </summary>
		/// <param name="title">The notes title</param>
        /// <param name="contents">The notes contents</param>
        /// <param name="colour">The notes colour</param>
        /// <param name="id">The notes ID</param>
        /// <param name="windowVisible">If the window should be instantly created or not</param>
        /// <param name="windowPosition">The position of the window</param>

		public Note(string title, string contents, Color colour, string id, bool windowVisible, Vector2 windowPosition)
		{
			Title = title;
			Contents = contents;
			Colour = colour;
			ID = id;
			WindowVisible = windowVisible;
			WindowPosition = windowPosition;
			
			CreatePreview();
			CreateWindow();

			if(!windowVisible)
				window.Hide();
		}

		/// <summary>
		/// Creates a brand new note
		/// </summary>
		/// <param name="id">The new GUID for the note</param>
		public Note(string id)
		{
			Title = "New Note";
			Contents = "";
			Colour = new Color(GD.RandRange(0, 255) / 255f, GD.RandRange(0, 255) / 255f, GD.RandRange(0, 255) / 255f);
			ID = id;
			WindowVisible = true;
			WindowPosition = Vector2.Zero;

			CreatePreview();
			CreateWindow();

			preview.Position = MainCamera._camera.Offset;

			WindowVisible = false;
			window.Hide();
		}

		/// <summary>
		/// Create the window for this note
		/// </summary>
		public void CreateWindow()
		{
			window = NoteManager.Instance.CreateWindow(this);
		}

		/// <summary>
		/// Creates the preview for this note
		/// </summary>
		void CreatePreview()
		{
			preview = NoteManager.Instance.CreatePreview(this);
		}

		public event Action OnTitleChanged;
		public event Action OnContentsChanged;
		public event Action OnColourChanged;
		public event Action OnConnectionCreate;
		public event Action OnConnectionDelete;
		public event Action OnNoteDelete;

		public void CallOnTitle()
		{
			OnTitleChanged?.Invoke();
		}
		public void CallOnContents()
		{
			OnContentsChanged?.Invoke();
		}
		public void CallConnectionCreate()
		{
			OnConnectionCreate?.Invoke();
		}
		public void CallConnectionDelete()
		{
			OnConnectionDelete?.Invoke();
		}
		public void CallNoteDelete()
		{
			OnNoteDelete?.Invoke();
		}
		public void CallOnColour()
		{
			OnColourChanged?.Invoke();
		}
	}
}
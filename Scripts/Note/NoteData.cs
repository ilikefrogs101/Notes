using Godot;
using System;

public partial class NoteData : Resource
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
	public NoteData(string title, string contents, Color colour, string id, bool windowVisible, Vector2 windowPosition)
	{
		Title = title;
		Contents = contents;
		Colour = colour;
		ID = id;
		WindowVisible = windowVisible;
		WindowPosition = windowPosition;
		
		CreatePreview();
		if(windowVisible)
			CreateWindow();
	}

	/// <summary>
    /// Creates a brand new note
    /// </summary>
	public NoteData(string id)
    {
        Title = "New Note";
        Contents = "";
        Colour = new Color(GD.RandRange(0, 255) / 255f, GD.RandRange(0, 255) / 255f, GD.RandRange(0, 255) / 255f);
        ID = id;
		WindowVisible = true;
		WindowPosition = Vector2.Zero;

		CreatePreview();
		CreateWindow();
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
        OnTitleChanged.Invoke();
    }
    public void CallOnContents()
    {
        OnContentsChanged.Invoke();
    }
    public void CallConnectionCreate()
    {
        OnConnectionCreate.Invoke();
    }
	public void CallConnectionDelete()
    {
        OnConnectionDelete.Invoke();
    }
    public void CallNoteDelete()
    {
        OnNoteDelete.Invoke();
    }
    public void CallOnColour()
    {
        OnColourChanged.Invoke();
    }
}

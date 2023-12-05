using Godot;
using System;

public partial class LineDrawer : Control
{
	public override void _PhysicsProcess(double delta)
	{
		QueueRedraw();
	}

    public override void _Draw()
    {
		// Fetch the connections and store
		// go through each connection
		// remove it if it is invalid
		// draw the line
		ConnectionData[] connections = NoteManager.Instance.GetConnections();
        foreach (ConnectionData connection in connections)
        {
			if(NoteManager.Instance.ShouldRemove(connection)) { NoteManager.Instance.RemoveConnection(connection); break; }

            DrawLine(
				connection.start.preview.GlobalPosition + connection.start.preview.Size, 
				connection.end.preview.GlobalPosition + connection.end.preview.Size,
				new Color(0, 0, 0), 
				2,
				true
			);
        }

		// Fetch the start of a new connection
		// check if it exists
		// draw line from the connection start to the mouse position
		NoteData _connectionStart = NoteManager.Instance.GetConnectionStart();
        if (_connectionStart != null)
        {
            DrawLine(
				_connectionStart.preview.GlobalPosition + _connectionStart.preview.Size, 
				GetViewport().GetMousePosition(), 
				new Color(0, 0, 0),
				2,
				true
			);
        }
    }
}

using Godot;
using System;
using System.Collections.Generic;
using ilikefrogs101.Notes.NoteComponents;
using ilikefrogs101.Notes.Data;
using System.Linq;

namespace ilikefrogs101.Notes.Manager
{
    public partial class NoteManager : Node2D
    {
        public static NoteManager Instance;

        // Node Paths
        private const string _notePath = "res://Prefabs/Note.tscn";
        private const string _previewPath = "res://Prefabs/Preview.tscn";
        private const string _previewHolderPath = "PreviewHolder";

        // Nodes and prefabs
        private PackedScene _noteScene;
        private PackedScene _previewScene;
        private Node _noteHolder;
        private Control _previewHolder;

        // Lists
        public readonly List<Note> _notes = new();
        private readonly List<ConnectionData> _connections = new();

        // Data
        private Note _connectionStart;

        public override void _Ready()
        {
            Instance = this;

            // Fetch and store nodes and prefabs from paths
            _noteScene = GD.Load<PackedScene>(_notePath);
            _previewScene = GD.Load<PackedScene>(_previewPath);
            _noteHolder = GetTree().CurrentScene;
            _previewHolder = _noteHolder.GetNode<Control>(_previewHolderPath);

            // Tell the preview holder to draw with our custom function
            _previewHolder.Draw += DrawConnections;
        }
        public override void _PhysicsProcess(double delta)
        {
            _previewHolder.QueueRedraw();
        }

        /// <summary>
        /// Creates a new note
        /// </summary>
        public void CreateNote()
        {
            // Generate the ID
            // create a new NodeData with the generated ID and add it to the list
            string id = Guid.NewGuid().ToString();
            Note data = new(id);
            _notes.Add(data);
        }

        /// <summary>
        /// Creates and returns the window for a note based on the given NoteData
        /// </summary>
        /// <param name="note">The note to create a window for</param>
        public NoteWindow CreateWindow(Note note)
        {
            // Instantiate the a new window and assign the data before adding as a child
            NoteWindow window = _noteScene.Instantiate<NoteWindow>();
            window.data = note;
            _noteHolder.AddChild(window);

            return window;
        }

        /// <summary>
        /// Creates and returns the preview for a note based on the given NoteData
        /// </summary>
        /// <param name="note">The note to create a preview for</param>
        public NotePreview CreatePreview(Note note)
        {
            // Instantiate a note preview, assign note data and add it as a child
            NotePreview preview = _previewScene.Instantiate<NotePreview>();
            preview.data = note;
            _previewHolder.AddChild(preview);

            return preview;
        }

        /// <summary>
        /// Creates a new connection or sets the connectionStart variable to the given NoteData
        /// </summary>
        /// <param name="note">The Note to either connect to or start a connection from</param>
        public void CreateConnection(Note note)
        {
            // If the connection start exists create a new connection otherwise set connection start to new data
            if (_connectionStart != null)
            {
                ConnectionData connectionData = new()
                {
                    start = _connectionStart,
                    end = note
                };
                _connections.Add(connectionData);
                _connectionStart = null;
            }
            else
            {
                _connectionStart = note;
            }
        }

        /// <summary>
        /// Returns true if a given connection is valid and false if it isnt
        /// </summary>
        /// <param name="connection">The connection to check the validity of</param>
        public bool ConnectionValid(ConnectionData connection)
        {
            return IsInstanceValid(connection.start.preview) && IsInstanceValid(connection.end.preview); 
        }

        /// <summary>
        /// Draws the list of connections between notes
        /// </summary>
        public void DrawConnections()
		{
			// Fetch the connections and store as an array
			// go through each connection
			// remove it if it is invalid
			// draw the line
			ConnectionData[] connections = _connections.ToArray();
			foreach (ConnectionData connection in connections)
			{
				if(!ConnectionValid(connection))
                { 
                    _connections.Remove(connection); 
                    break; 
                }

				_previewHolder.DrawLine(
					connection.start.preview.Position + connection.start.preview.Size, 
					connection.end.preview.Position + connection.end.preview.Size,
					new Color(0, 0, 0), 
					2,
					true
				);
			}

			// Check if the user is creating a new connection
			// draw line from the connection start to the mouse position
			if (_connectionStart != null)
			{
				_previewHolder.DrawLine(
					_connectionStart.preview.Position + _connectionStart.preview.Size, 
					_previewHolder.GetLocalMousePosition(),
					new Color(0, 0, 0),
					2,
					true
				);
			}
		}
    }
}
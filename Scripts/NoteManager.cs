using Godot;
using System;
using System.Collections.Generic;
using ilikefrogs101.Notes.NoteComponents;
using ilikefrogs101.Notes.Data;

namespace ilikefrogs101.Notes.Manager
{
    public partial class NoteManager : Node2D
    {
        public static NoteManager Instance;

        // Node Paths
        private string _notePath = "res://Prefabs/Note.tscn";
        private string _previewPath = "res://Prefabs/Preview.tscn";
        private string _previewHolderPath = "CanvasLayer/PreviewHolder";

        // Nodes and prefabs
        private PackedScene _noteScene;
        private PackedScene _previewScene;
        private Node _noteHolder;
        private Node _previewHolder;

        // Lists
        private List<Note> _notes = new();
        private List<ConnectionData> _connections = new();

        // Data
        Note _connectionStart;

        public override void _Ready()
        {
            Instance = this;

            // Fetch and store nodes and prefabs from paths
            _noteScene = GD.Load<PackedScene>(_notePath);
            _previewScene = GD.Load<PackedScene>(_previewPath);
            _noteHolder = GetTree().CurrentScene;
            _previewHolder = _noteHolder.GetNode(_previewHolderPath);
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
        public NoteWindow CreateWindow(Note data)
        {
            // Instantiate the a new window and assign the data before adding as a child
            NoteWindow window = _noteScene.Instantiate<NoteWindow>();
            window.data = data;
            _noteHolder.AddChild(window);

            return window;
        }

        /// <summary>
        /// Creates and returns the preview for a note based on the given NoteData
        /// </summary>
        public NotePreview CreatePreview(Note data)
        {
            // Instantiate a note preview, assign note data and add it as a child
            NotePreview preview = _previewScene.Instantiate<NotePreview>();
            preview.data = data;
            _previewHolder.AddChild(preview);

            return preview;
        }

        /// <summary>
        /// Creates a new connection or sets the connectionStart variable to the given NoteData
        /// </summary>
        public void CreateConnection(Note data)
        {
            // If the connection start exists create a new connection otherwise set connection start to new data
            if (_connectionStart != null)
            {
                ConnectionData connectionData = new()
                {
                    start = _connectionStart,
                    end = data
                };
                _connections.Add(connectionData);
                _connectionStart = null;
            }
            else
            {
                _connectionStart = data;
            }
        }

        /// <summary>
        /// Returns the list of connections
        /// </summary>
        public ConnectionData[] GetConnections()
        {
            return _connections.ToArray();
        }

        /// <summary>
        /// Returns the connection start variable
        /// </summary>
        public Note GetConnectionStart()
        {
            return _connectionStart;
        }

        /// <summary>
        /// Returns whether a certain connection should be removed due to a preview not existing
        /// </summary>
        public bool ShouldRemove(ConnectionData connection)
        {
            return !IsInstanceValid(connection.start.preview) || !IsInstanceValid(connection.end.preview); 
        }

        /// <summary>
        /// Removes a connection from the list of connections
        /// </summary>
        public void RemoveConnection(ConnectionData connection)
        {
            if(ConnectionExists(connection))
                _connections.Remove(connection);
        }

        /// <summary>
        /// Returns whether a connection exists
        /// </summary>
        public bool ConnectionExists(ConnectionData connection)
        {
            return _connections.Contains(connection);
        }
    }
}
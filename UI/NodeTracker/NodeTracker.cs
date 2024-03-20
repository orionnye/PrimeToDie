using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class NodeTracker : RichTextLabel
{
	[Export] public Node3D target;
	[Export] public Camera3D cam;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		if (cam == null) {
			cam = (Camera3D)GetParent();
		}
		// target = target == null ? null : (Node3D)GetParent();
		// Visible = false;
	}

	public void Track() {
		if (cam != null) {
			Vector2 pos = cam.UnprojectPosition(target.GlobalPosition);
			GlobalPosition = pos;
			// Set("position", pos);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		Godot.Collections.Array<Godot.Node> players = GetTree().GetNodesInGroup("Player");
		if (target != null) {
			Track();
		}
	}
}

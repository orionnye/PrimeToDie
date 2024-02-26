using Godot;
using System;

public partial class PlayerCam : Camera3D
{
	Node3D target;
	Vector3 fixedPerspective;

	public Vector2 GetMousePosInViewport() {
		// Gets the mouse position on screen
		Vector2 mouseInViewport = GetViewport().GetMousePosition();
		return mouseInViewport;
	}
	public void setMouseText(String text) {
		RichTextLabel mouseMarker = GetNode<RichTextLabel>("MouseMarker");
		mouseMarker.Position = GetMousePosInViewport();
		mouseMarker.Text = text;
	}

	public Vector2 GetNodePosInViewport(Node3D node) {
		Vector2 nodeInViewport = UnprojectPosition(node.GlobalPosition);
		// Gets the mouse position on screen
		return nodeInViewport;
	}
	public void updateNode(Vector2 pos, RichTextLabel label, String text) {;
		label.Position = pos;
		label.Text = text;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// GD.Print("Boogus");
		fixedPerspective = this.Position;
		target = (Node3D)GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		this.Set("global_position", fixedPerspective + target.Position);
		this.Set("global_rotation", new Vector3(Rotation.X, 0, Rotation.Z));
	}
}

using Godot;
using System;

public partial class TopDownCam : RigidBody3D
{
	Vector3 desiredOffset = new Vector3(0, 20, 0);
	Vector3 desiredRotation = new Vector3(90, 0, 0);
	public float speed = 0.001f;

	[Export] public MeshInstance3D mouseMarker;
	[Export] public ColorRect mouseTag;
	[Export] public Camera3D cam;
	public bool isTrackingMouse = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		desiredOffset = Position;
	}

	// Utility functions

	//                 |
	// Mouse tracking \|/
	//                 V
	public Vector2 GetMousePosInViewport() {
		// Gets the mouse position on screen
		Vector2 mouseInViewport = GetViewport().GetMousePosition();
		return mouseInViewport;
	}
	public Vector3 GetMousePosInScene() {
		return cam.ProjectPosition(GetMousePosInViewport(), desiredOffset.Y);
	}

	//                 |
	//      Motion    \|/
	//                 V
	public Vector3 track() {
		Node3D parent = (Node3D)GetParent();
		Vector3 direction = (parent.GlobalPosition + desiredOffset - GlobalPosition).Normalized();
		Vector3 motion = direction*speed;
		return motion;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		Node3D parent = (Node3D)GetParent();
		GlobalPosition = GlobalPosition.Lerp(parent.GlobalPosition + desiredOffset, 0.01f);
		Vector3 mousePos = GetMousePosInScene();
		mousePos.Y = 1;
		mouseMarker.GlobalPosition = mousePos;
		mouseTag.Position = GetMousePosInViewport();
	}
}

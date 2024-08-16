using Godot;
using System;

public partial class TopDown : Node3D
{
	// Camera
	[Export] public Camera3D cam;
	
	// Focus/ Targets
	[Export] public Node3D target;
	[Export] private Vector3 range = new Vector3(0.5f, 0.5f, 0.5f);

	// cameraCollider
	[Export] private RigidBody3D cameraCollider;

	Vector3 desiredOffset = new Vector3(0, 20, 0);
	Vector3 desiredRotation = new Vector3(90, 0, 0);
	public float speed = 0.001f;

	// Camera UI
	[Export] public MeshInstance3D mouseMarker;
	[Export] public ColorRect mouseTag;
	private Vector3 mousePos;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		desiredOffset = cameraCollider.Position;
	}

	// Utility functions

	//                 |
	// Mouse tracking \|/
	//                 V
	public Vector2 GetMousePosInViewport() {
		// Gets the mouse position on screen
		Vector2 mouseInViewport = cam.GetViewport().GetMousePosition();
		return mouseInViewport;
	}

	public Vector3 GetMousePosInScene() {
		return cam.ProjectPosition(GetMousePosInViewport(), desiredOffset.Y);
	}

	// Node specific camera functions
	public Vector2 GetMouseRelativeToNode(Node3D node) {
		// Gets the mouse position on screen
		Vector2 mouseInViewport = GetViewport().GetMousePosition();
		// Gets the node Position in ViewPort
		Vector2 nodeInViewport = cam.UnprojectPosition(node.GlobalPosition);
		// Compares mouse position in viewport compared to nodePos in viewport
		Vector2 mouseRelative = nodeInViewport - mouseInViewport;
		// return the comparison
		return mouseRelative;
	}

	public Vector3 GetMouseRotation(Node3D node) {
		// Get player rotation based on mouse
		Vector2 mouseRelative = GetMouseRelativeToNode(node);
		Vector3 mouseInSpace = new Vector3(mouseRelative.X, GlobalPosition.Y, mouseRelative.Y);

		return mouseInSpace;
	}

	//                 |
	//      Motion    \|/
	//                 V
	public void track() {
		cameraCollider.GlobalPosition = cameraCollider.GlobalPosition.Lerp(GlobalPosition + desiredOffset, 0.01f);
		if (target != null) {
			GlobalPosition = target.GlobalPosition;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// Motion and tracking
		track();

		// Mouse tracking and display
		Vector3 mousePos = GetMousePosInScene();
		mousePos.Y = 1;
		mouseMarker.GlobalPosition = mousePos;
		mouseTag.Position = GetMousePosInViewport();
	}
}

using Godot;
using System;

public partial class Follow : Node3D
{
	[Export] public Node3D target;
	[Export] private Vector3 range = new Vector3(0.5f, 0.5f, 0.5f);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (target != null ) {
			GlobalPosition = GlobalPosition.Lerp(target.GlobalPosition, 0.01f);
		}
	}
}

using Godot;
using System;

public partial class Squished : Unit
{
	[Export] public new float speed = 0.3f;
	private float speedStore;
	public bool isFlipped() {
		bool flipped = false;
		if (RotationDegrees.X < -100 && RotationDegrees.X > 100) {
			flipped = true;
		}
		else if (RotationDegrees.Y < -100 && RotationDegrees.Y > 100) {
			flipped = true;
		}
		else if (RotationDegrees.Z < -100 && RotationDegrees.Z > 100) {
			flipped = true;
		}
		return flipped;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// get animation playing on character
		speedStore = speed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// GD.Print("Jaboogah");
		if (isFlipped()) {
			speed = 0;
		} else {
			speed = speedStore;
		}
	}
}

using Godot;
using System;

public partial class User : Unit
{
	public bool focus = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	private Vector3 GetMotion() {
		Vector3 velocity = Vector3.Zero;

		float speed = 0.5F;
		// Handle movement input
		if (Input.IsActionPressed("w")) {
			velocity.Z -= 1;
		}
		if (Input.IsActionPressed("a")) {
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("s")) {
			velocity.Z += 1;
		}
		if (Input.IsActionPressed("d")) {
			velocity.X += 1;
		}
		return velocity.Normalized()*speed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);
		Vector3 motionInput = GetMotion();
		// Mouse Functions
		// this.LookAt(GetMouseRotation());

		// dampen motion during in-action
		if (motionInput.Length() == 0) {
			// GD.Print("NO USER INPUT");
			Inertia = Inertia.Lerp(Vector3.Zero, 0.99F);
			// LinearVelocity = Vector3.Zero;
			LinearDamp = 0.99F;
		}
		ApplyCentralImpulse(motionInput);
	}
}

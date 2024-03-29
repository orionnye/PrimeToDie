using Godot;
using System;

public partial class TopDownCam : RigidBody3D
{
	Vector3 desiredOffset = new Vector3(0, 20, 0);
	Vector3 desiredRotation = new Vector3(90, 0, 0);
	public float speed = 0.001f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		desiredOffset = Position;
	}

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
	}
	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		// base._IntegrateForces(state);
		// Vector3 motionInput = track();
		// // dampen motion during in-action
		// if (motionInput.Length() == 0) {
		// 	LinearDamp = 0.59F;
		// 	Inertia = Inertia.Lerp(Vector3.Zero, LinearDamp); //This is the impact of friction improperly implented
		// 	LinearVelocity = Vector3.Zero;
		// }
		// ApplyCentralImpulse(motionInput);
	}
}

using Godot;
using System;
using System.Linq;

public partial class Glass : RigidBody3D
{
	
	// Lots of stuff about the glass and ai

	// Have ai do the ai thing and control stuff

	// Probably need a function that emits signals to behave in certain ways

	// This is a root class for AI, needs access to nearby units and exert influence over them?

	// Or should it passively be called?

	// Passive or active AI?
	[Export] float speed = 5f;
	[Export] Unit tempPlayer;
	[Export] bool isUser = false;
	bool hosted = false;

	private Vector3 GetMotion() {
		Vector3 velocity = Vector3.Zero;

		float speed = 0.5F;
		if (this.isUser) {
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
		}
		return velocity.Normalized()*speed;
	}

	// Time to create initial glass behavior, it wants to be picked up
	// From a programming perspective, let's implement how we want it to work in game
	// The Glass Moves to be picked up, the glass mounts itself.
	// Let the functionality reflect the lore
	public Unit findHost() {
		
		Unit target = null;
		
		// Search Root for hosts
		Unit[] hosts = (Unit[])GetTree().GetNodesInGroup("Unit").ToArray();

		// Select Host based off criteria
		float closest = 100;
		// Distance for now
		foreach (Unit host in hosts) {
			float distance = (host.GlobalPosition - GlobalPosition).Length();
			// Distance check
			if (distance < closest) {
				closest = distance;
				target = host;
			}
		}
		// Return selected Unit or return null if no desirable Hosts

		return target;
	}

	public void seek(Unit target) {
		Vector3 distance = target.GlobalPosition - GlobalPosition;
		ApplyCentralForce(distance.Normalized()*speed);
	}

	public void changeHost(Unit host) {
		// Pass in valid host, change host from current/or none to new host
		Reparent(host.bank, true);

		// Add some noise for diversity and reset position for new parent
		RandomNumberGenerator rng = new RandomNumberGenerator();
		Vector3 tempDistribute = new Vector3(1.3f, 1, 0.3f);
		Position = (new Vector3(rng.Randf(), rng.Randf(), 1)*tempDistribute) - (tempDistribute/2);
		Rotation = new Vector3(rng.Randf(), rng.Randf(), rng.Randf());

		// Making glass inert and idle to not mess with Unit controls
		Freeze = true;
		SetPhysicsProcess(false);
		// Sleeping = true;
		hosted = !hosted;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// If disembodied,
		// if (tempPlayer != null) {
		// 	// GD.Print("player is valid");
		// 	seek(tempPlayer);

		// }
		// Find host, seek host
		// if (GetParent().GetType() == typeof(Unit)) {
		// 	// If bodied, control host
		// 	// Issue commands here
		// 	GD.Print("occupying host");
		// } else {
			// GD.Print("not occupying host");
			// Unit host = findHost();
			// GD.Print("Host:", host);

			// if (host != null) {
			// 	GD.Print("found host:", host);
			// 	seek(host);
			// }
		// }
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		// if (tempPlayer != null) {
		// 	// GD.Print("player is valid");
		// 	seek(tempPlayer);
		// }
	}
	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);

		// captures motion
		Vector3 motionInput = GetMotion();

		// dampen motion during in-action
		if (motionInput.Length() == 0) {
			// GD.Print("NO USER INPUT");
			Inertia = Inertia.Lerp(Vector3.Zero, 0.99F);
			// LinearVelocity = Vector3.Zero;
			LinearDamp = 0.99F;
		}
		ApplyCentralImpulse(motionInput);
		
	}
	// Signal Handling
	private void _on_area_3d_body_entered(Node3D body)
	{
		if (body.GetType() == typeof(Unit)) {
			// GD.Print("I touched a Unit!!");
			changeHost((Unit)body);
		}
	}
}







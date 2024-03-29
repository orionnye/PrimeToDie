using Godot;
using System;

public partial class Unit : RigidBody3D
{
	// Declare default Unit stats, movement speed, turn speed, etc.
	// Character interaction stats
	[Export] private int hp = 10; // Health Points storage
	[Export] private int mp = 10; // Max Health Points storage

	// use this to define target direction and derive motion
	[Export] Node3D target;
	[Export] private float speed = 0.5f;
	[Export] private float rotationSpeed = 0.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}
	// Getter and Setter method for private
	public void modifyHealthPoints(int impact) {
		// this method serves as a access point for Unit stats and could potentially have checks and overrides depending on item features
		this.hp -= impact;
	}
	public void modifyMaxPoints(int impact) {
		// this method serves as a access point for Unit stats and could potentially have checks and overrides depending on item features
		this.mp -= impact;
	}
	// take damage requires a bullet but should accept any physics object and determine the damage based off of impact to inertia
	public void takeDamage(Bullet bullet) {
		hp -= (int)bullet.damage;
		if ( hp <= 0 ) {
			die();
		}
	}

	public void die() {
		// drops materials before death
		// trigger potential on death effects
		this.QueueFree();
	}

	// Speed and Motion Controls
	public Vector3 GetMotion() {
		Vector3 motion = new Vector3(0, 0, 0);
		// Motion should take into account rotation, for now we'll leave that to inertia
		if ( target != null ) {
			Vector3 direction = (target.GlobalPosition - GlobalPosition).Normalized();
			motion = direction*speed;
		}
		return motion;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
	// call physics process overload
	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);
		Vector3 motionInput = GetMotion();
		// // Mouse Functions
		// create invisible 3D node that follows mouse position and pass it into here for player controls
		// this.LookAt(GetMouseRotation());

		// dampen motion during in-action
		if (motionInput.Length() == 0) {
			// GD.Print("NO USER INPUT");
			LinearDamp = 0.99F;
			Inertia = Inertia.Lerp(Vector3.Zero, LinearDamp); //This is the impact of friction improperly implented
			// LinearVelocity = Vector3.Zero;
		}
		ApplyCentralImpulse(motionInput);
	}
}

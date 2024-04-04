using Godot;
using System;

public partial class Unit : RigidBody3D
{
	// Declare default Unit stats, movement speed, turn speed, etc.
	// Character interaction stats
	[Export] private int hp = 10; // Health Points storage
	[Export] private int mp = 10; // Max Health Points storage

	// Item storage and handling
	[Export] public Node3D hands;
	[Export] private int holdLimit = 1;
	[Export] private float handReach = 1f;
	public bool isHolding() {return hands.GetChildCount() > 0;}
	public bool isHandsFull() {return hands.GetChildCount() >= holdLimit;}

	// use this to define target direction and derive motion
	[Export] Node3D target;
	[Export] private float speed = 0.5f;
	[Export] private float rotationSpeed = 0.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Event Listeners
	public void _on_hand_collider_body_entered(Node3D body) {
		// GD.Print("Something collided for sure");
		// GD.Print("type:", body.GetType());
		if (body.IsInGroup("Items")) {
			GD.Print("We could grab this item!!!");
			Grab((Item)body);
		}
	}
	public void _on_timer_timeout() {

	}

	// Define Grab, and Drop Functions.
	public Item isItemInReach() {
		// Find root children matching "Item" category
		return null;
	}
	public void Grab(Item item) {
		// Grab Item function, assigns item to the hand and removes it from root
		if (isHolding()) {
			Drop();
		}
		item.Reparent(hands, true);
		item.Position = item.heldPosition;
		item.Rotation = item.heldRotation;
		item.Set("freeze", true);
	}
	public void Drop() {
		// Drops Item function, assigns item to the root and removes it from hands
		if (isHolding()) {
			Node3D item = hands.GetChild<Item>(0);
			item.Set("freeze", false);
			item.Reparent(GetTree().Root, true);
		}
	}

	// Getter and Setter method for private properties
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
		QueueFree();
	}
	

	// Speed and Motion Controls
	private Vector3 GetMotion() {
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

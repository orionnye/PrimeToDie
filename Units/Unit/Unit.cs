using Godot;
using System;
using System.Linq;

public partial class Unit : RigidBody3D
{
	// Character interaction stats
	[Export] private int hp = 10; // Health Points storage
	[Export] private int mp = 10; // Max Health Points storage

	// Item storage and handling
	[Export] public Node3D hands;
	[Export] private int holdLimit = 1;
	[Export] private float handReach = 1f;
	[Export] public Node3D bank;
	[Export] public float magnetism = 0.1f;

	// Access functions
	public bool isHolding() {return (hands == null || hands.GetChildCount() > 0);}
	public bool isHandsFull() {return (hands == null || hands.GetChildCount() >= holdLimit);}
	public Item GetItem() {return hands.GetChild<Item>(0);}

	// use this to define target direction and derive motion
	public Vector3 focus;
	[Export] Node3D target;
	[Export] public float speed = 0.5f;
	[Export] public float rotationSpeed = 0.1f;

	// AI Controls or User Controls with emitters control signals
	// [Export] public Controller controller;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		bank = GetNode<Node3D>("Bank");
	}

	// Event Listeners
	public void _on_hand_collider_body_entered(Node3D body) {
	// 	// GD.Print("Something collided for sure");
	// 	// GD.Print("type:", body.GetType());
	// 	if (body.IsInGroup("Items")) {
	// 		GD.Print("We could grab this item!!!");
	// 		Grab((Item)body);
	// 	}
	}

	// Define Grab, and Drop Functions.
	public Item isItemInReach() {
		// Find root children matching "Item" category
		return null;
	}
	public void Grab(Item item) {
		// Grab Item function, assigns item to the hand and removes it from root
		if (!isHolding()) {
			item.Reparent(hands, true);
			item.Position = item.heldPosition;
			item.Rotation = item.heldRotation;
			item.Set("freeze", true);
		}
	}
	public void Drop() {
		// Drops Item function, assigns item to the root and removes it from hands
		if (isHolding()) {
			Node3D item = hands.GetChild<Item>(0);
			item.Set("freeze", false);
			item.Reparent(GetTree().Root, true);
		}
	}
	public void Use(Item item) {
		if (isHolding()) {
			if (item.active) {
				item.Deactivate();
			} else {
				item.Activate();
			}
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
	// Take damage requires a bullet but should accept any physics object and determine the damage based off of impact to inertia
	public void takeDamage(Bullet bullet) {
		hp -= (int)bullet.damage;
		if ( hp <= 0 ) {
			die();
		}
	}

	//Death Handling
	public void die() {
		// drops materials before death
		// trigger potential on death effects
		Drop();
		Godot.Collections.Array<Godot.Node> resources = bank.GetChildren();
		foreach (RigidBody3D resource in resources)
		{
			resource.Reparent(GetTree().Root, true);
			resource.Freeze = false;
			resource.SetPhysicsProcess(true);
			resource.SetProcess(true);
		}
		QueueFree();
	}

	// // Glass Interaction
	// public void attractGlass() {
	// 	Node[] glassUnclaimed = GetTree().GetNodesInGroup("Glass").ToArray();
	// 	foreach (Glass item in glassUnclaimed) {
	// 		Vector3 distance = GlobalPosition - item.GlobalPosition;
	// 		item.ApplyCentralForce(distance.Normalized()*magnetism);
	// 		// GD.Print(distance);
	// 	}
	// }

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
	public Vector3 GetRotation() {
		// Get Rotation on player and set it
		return new Vector3(0, 0, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// attractGlass();
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

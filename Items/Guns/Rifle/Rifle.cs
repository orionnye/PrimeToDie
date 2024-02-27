using Godot;
using System;

public partial class Rifle : Item
{
	[Export] public int bulletCount = 1;
	[Export] public float bulletSpeed = 3;
	// Global transform.basis
	public Vector3 barrelExit;
	static PackedScene bulletScene = GD.Load<PackedScene>("res://Items/Guns/Bullet.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		heldPosition = new Vector3(0.7f, 0.5f, 0);
		barrelExit = GetNode<Node3D>("BarrelExit").Position;
	}

	public override void Activate() {
		// GD.Print("Activating Rifle");
		for (int i = 0; i < bulletCount; i++) {
			Bullet bullet = (Bullet)bulletScene.Instantiate();
			AddChild(bullet);
			// Calculate Gun Length and then rotate it according to globalRotation(export as own function)
			bullet.Position = barrelExit;
			bullet.Reparent(GetTree().Root, true);
			// bullet.ApplyCentralImpulse(new Vector3(0, 0, bulletSpeed));
			bullet.ApplyCentralImpulse(GlobalTransform.Basis.Z * bulletSpeed);
		}
	}

	public override void Deactivate() {
		GD.Print("no deactivate functions");
		// throw new NotImplementedException();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}

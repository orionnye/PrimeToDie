using Godot;
using System;

public partial class BlockEnemy : RigidBody3D
{
	[Export] public Node3D target;
	[Export] public float speed = 0.3f;
	[Export] public float hp = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		
	}

	public void follow() {
		if ( target != null ) {
			// Position = Position.Lerp(target.Position, 0.1f);
			Vector3 direction = (target.Position - Position).Normalized();
			this.ApplyCentralImpulse(direction*speed);
		}
	}
	public void takeDamage(Bullet bullet) {
		hp -= bullet.damage;
		if ( hp <= 0 ) {
			die();
		}
	}
	public void die() {
		// drops materials before death
		GD.Print("kill me!!!!");
		this.QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// follow();
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		follow();
	}
}

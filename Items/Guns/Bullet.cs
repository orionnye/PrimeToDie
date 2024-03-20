using Godot;
using System;

public partial class Bullet : RigidBody3D
{
	[Export] public float timerDirty = 0;
	[Export] public float timerMaxDirty = 10;
	[Export] public float damage = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (timerDirty < timerMaxDirty) {
			timerDirty += (float)delta;
		} else {
			QueueFree();
		}
	}

	private void _on_body_entered(Node body)
	{
		// On hit function
		if (body.HasMethod("die")) {
			BlockEnemy enemy = (BlockEnemy)body;
			enemy.takeDamage(this);
			// enemy.die();
			this.QueueFree();
		}
	}
}



using Godot;
using System;

public partial class Spawner : StaticBody3D
{
	[Export] public PackedScene spawnItem = GD.Load<PackedScene>("res://Items/Blocks/Blink.tscn");
	[Export] private float spawnRadius = 5;
	[Export] private double maxTimer = 10;
	[Export] public Node3D target;
	private double timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// perhaps this should bring up a small ui prompt on the object to say what it wants
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// Setup timer
		if (timer <= maxTimer) {
			timer += delta;
		} else {
			// Trigger timeout effect here
			on_timeout();
			timer = 0;
		}
	}
	private void on_timeout() {
		GD.Print("Spawning Entity");
		Node3D entity = (Node3D)spawnItem.Instantiate();
		Vector3 offset = new Vector3(1, 0, 1) * (float)GD.RandRange(-spawnRadius, spawnRadius);
		offset.Y = 1;
		entity.Position = this.GlobalPosition + offset;
		if (entity.HasMethod("follow")) {
			BlockEnemy enemy = (BlockEnemy)entity;
			enemy.target = target;
			GetTree().Root.AddChild(enemy);
			// GD.Print("spawning enemy with a target!");
		} else {
			GetTree().Root.AddChild(entity);
		}
	}
}

using Godot;
using System;

public partial class Blink : Item
{
	// Storage and Translation
	double timer = 0;
	double cooldown = 0.5;
	[Export] public float inertMass = 1f;
	[Export] public float activeMass = 10f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		active = false;
		Set("mass", inertMass);

		activeMaterial = new StandardMaterial3D{ AlbedoColor = new Color(1f, 0f, 0f) };
		inactiveMaterial = new StandardMaterial3D{ AlbedoColor = new Color(1f, 1f, 1f) };
	}
	
	// Base Activate function
	public override void Activate() {
		// GD.Print("Activating: ", this);
		active = true;
		Set("mass", activeMass);
		GetNode("MeshInstance3D").Set("surface_material_override/0", activeMaterial);
	}
	// Base Inactive function
	public override void Deactivate() {
		// GD.Print("Deactivating: ", this);
		active = false;
		Set("mass", inertMass);
		GetNode("MeshInstance3D").Set("surface_material_override/0", inactiveMaterial);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if (active) {
			// timer += delta;
			// if (timer >= cooldown) {
			// 	timer = 0;
			// 	Deactivate();
			// }
		// } else {
			// timer += delta;
			// if (timer > cooldown) {
			// 	timer = 0;
			// 	Activate();
			// }
		// }

	}
}

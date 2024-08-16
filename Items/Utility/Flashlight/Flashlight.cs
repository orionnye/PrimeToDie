using Godot;
using System;

public partial class Flashlight : Item
{
	[Export] public float energy = 5;
	[Export] public Vector3 heldPosition;
	[Export] public Vector3 heldRotation;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Deactivate();

		// UI
		activeMaterial = new StandardMaterial3D{ AlbedoColor = new Color(1f, 0f, 0f) };
		inactiveMaterial = new StandardMaterial3D{ AlbedoColor = new Color(1f, 1f, 1f) };
	}


	// Base Activate function
	public override void Activate() {
		// GD.Print("Activating: ", this);
		active = true;
		GetNode<SpotLight3D>("Body/SpotLight3D").LightEnergy = energy;
	}
	// Base Inactive function
	public override void Deactivate() {
		// GD.Print("Deactivating: ", this);
		active = false;
		GetNode<SpotLight3D>("Body/SpotLight3D").LightEnergy = 0;
		// GetNode("MeshInstance3D").Set("surface_material_override/0", inactiveMaterial);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}

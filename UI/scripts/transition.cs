using Godot;
using System;

public partial class transition : Node
{
	[ Export ] PackedScene toScene; 

	// Add a method to change the scene
	public void ChangeToScene2()
	{
		// Instance the new scene
		Node newSceneInstance = toScene.Instantiate();

		// Add the new scene as a child of the root

		GetTree().Root.AddChild(newSceneInstance);

		// Free the old scene (optional)
		this.GetParent().QueueFree();

		GD.Print("Changed to new Scene");
	}
	private void _on_pressed()
	{
		// Replace with function body.
		ChangeToScene2();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}



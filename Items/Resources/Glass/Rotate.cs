using Godot;
using System;

public partial class Rotate : OmniLight3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RotateX(0.05f);
		RotateZ(0.04f);
		RotateY(0.03f);
	}
}

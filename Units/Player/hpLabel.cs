using Godot;
using System;

public partial class hpLabel : ColorRect
{
	[Export] public Node3D target; 
	[Export] PlayerCam cam; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// update postion to reflect posInCam
		if (target != null && cam != null) {
			Vector2 targetPosInCam = cam.UnprojectPosition(target.GlobalPosition);
			// GD.Print(targetPosInCam);
			this.GlobalPosition = targetPosInCam;
		}
	}
}

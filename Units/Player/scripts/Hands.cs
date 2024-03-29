using Godot;
using System;

public partial class Hands : Node3D
{
	PlayerCam cam;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// starting values

		// reference values
		cam = GetParent<Controller>().cam;

		// starting mutations
		foreach (var item in GetChildren()) {
			item.Set("freeze", true);
		}
	}
	// Reference function
	public Boolean isHolding() { return GetChildCount() > 0; }

	//                    |
	//                    |
	// Utility Functions \|/
	// 					  v


	// Uses the object
	public void Use(Item item) {
		if (isHolding()) {
			if (item.active) {
				item.Deactivate();
			} else {
				item.Activate();
			}
		}
	}
	
	// Grab Object
	public void Grab(Item item) {
		if (!isHolding()) {
			item.Reparent(this, true);
			item.Position = item.heldPosition;
			item.Rotation = item.heldRotation;
			item.Set("freeze", true);
		}
	}
	// Drop Object Held in hands
	public void Drop() {
		if (isHolding()) {
			Node3D item = (Node3D)GetChild(0);
			item.Set("freeze", false);
			item.Reparent(GetTree().Root, true);
		}
	}

	public Item getTargetItem() {
		foreach (Item item in GetTree().GetNodesInGroup("Items")) {
			if ((item.GlobalPosition - GlobalPosition).Length() < 2) {
				return item;
			}
		}
		return null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Item target = getTargetItem();
		// if (target != null) {
		// 	cam.TextOnNode(target);
		// }
		if (isHolding() && Input.IsActionJustReleased("Space")) {
			Use((Item)GetChild(0));
		}
		if (Input.IsActionJustReleased("Shift")) {
			if (isHolding()) {
				Drop();
			} else {
				if (target != null) {
					Grab(target);
				}
				// Restructure this design to return the object closest and add a UI element in the viewport over it
			}
		}
	}
}

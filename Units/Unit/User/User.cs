using Godot;
using System;

public partial class User : Unit
{
	public Vector3 focus = new Vector3(0, 0, 0);
	// Called when the node enters the scene tree for the first time.

	public override void _Ready() {
	}
	// public Vector3 GetMouseRotation() {
	// 	// Get player rotation based on mouse
	// 	Vector2 mouseRelative = GetMouseRelative();
	// 	Vector3 mouseInSpace = new Vector3(mouseRelative.X, GlobalPosition.Y, mouseRelative.Y);

	// 	return mouseInSpace;
	// }

	// public Vector2 GetMouseRelative() {
	// 	// Gets the mouse position on screen
	// 	Vector2 mouseInViewport = GetViewport().GetMousePosition();
	// 	// Gets the player Position in ViewPort
	// 	Vector2 playerInViewport = cam.UnprojectPosition(GlobalPosition);
	// 	// Compares mouse position in viewport compared to playerPos in viewport
	// 	Vector2 mouseRelative = playerInViewport - mouseInViewport;
	// 	// return the comparison
	// 	return mouseRelative;
	// }

	private Vector3 GetMotion() {
		Vector3 velocity = Vector3.Zero;

		float speed = 0.5F;
		// Handle movement input
		if (Input.IsActionPressed("w")) {
			velocity.Z -= 1;
		}
		if (Input.IsActionPressed("a")) {
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("s")) {
			velocity.Z += 1;
		}
		if (Input.IsActionPressed("d")) {
			velocity.X += 1;
		}
		return velocity.Normalized()*speed;
	}

	public Vector3 GetMouseRotation() {
		// Get player rotation based on mouse
		Vector2 mouseRelative = GetMouseRelative();
		Vector3 mouseInSpace = new Vector3(mouseRelative.X, GlobalPosition.Y, mouseRelative.Y);

		return mouseInSpace;
	}

	public Vector2 GetMouseRelative() {
		// Gets the mouse position on screen
		Vector2 mouseInViewport = GetViewport().GetMousePosition();
		// Gets the player Position in ViewPort
		Vector2 playerInViewport = GetViewport().GetCamera3D().UnprojectPosition(GlobalPosition);
		// Compares mouse position in viewport compared to playerPos in viewport
		Vector2 mouseRelative = playerInViewport - mouseInViewport;
		// return the comparison
		return mouseRelative;
	}


	public Vector3 GetRotation() {
		// Get Rotation from all AI's
		
		// currently: get focus and then look at it
		// We'll do this manually with the "lookat" function but we need to replace this so rotation isn't guarenteed
		return focus;
	}

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

	// Finds the object
	public Item getTargetItem() {
		foreach (Item item in GetTree().GetNodesInGroup("Items")) {
			if ((item.GlobalPosition - GlobalPosition).Length() < 2) {
				return item;
			}
		}
		return null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		Item target = getTargetItem();
		if (isHolding() && Input.IsActionJustReleased("Space")) {
			// Get child item from hands
			Use(GetItem());
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
	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);
		Vector3 motionInput = GetMotion();
		// Mouse Functions
		Vector2 mousePos = GetMouseRelative();
		Vector3 inSpaceMouse = new Vector3(mousePos.X, 1, mousePos.Y);
		this.LookAt(inSpaceMouse);

		// dampen motion during in-action
		if (motionInput.Length() == 0) {
			// GD.Print("NO USER INPUT");
			Inertia = Inertia.Lerp(Vector3.Zero, 0.99F);
			// LinearVelocity = Vector3.Zero;
			LinearDamp = 0.99F;
		}
		ApplyCentralImpulse(motionInput);
	}
}

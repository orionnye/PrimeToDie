using Godot;
using System;

public
partial class Controller : RigidBody3D {
  [Export] public PlayerCam cam;
  Vector3 startRot;

  // Called when the node enters the scene tree for the first time.
public
  override void _Ready() {
    cam = GetNode<PlayerCam>("Camera3D");
    startRot = Rotation;
    // DisplayServer.MouseSetMode(DisplayServer.MouseMode.Hidden);
  }

public
  Vector3 GetMotion() {
    Vector3 velocity = Vector3.Zero;

    float speed = 0.5F;
    // Handle movement input
    if (Input.IsActionPressed("d")) {
      velocity.X += 1;
    }
    if (Input.IsActionPressed("a")) {
      velocity.X -= 1;
    }
    if (Input.IsActionPressed("s")) {
      velocity.Z += 1;
    }
    if (Input.IsActionPressed("w")) {
      velocity.Z -= 1;
    }
    return velocity.Normalized() * speed;
  }

public
  Vector3 GetMouseRotation() {
    // Get player rotation based on mouse
    Vector2 mouseRelative = GetMouseRelative();
    Vector3 mouseInSpace =
        new Vector3(mouseRelative.X, GlobalPosition.Y, mouseRelative.Y);

    return mouseInSpace;
  }

public
  Vector2 GetMouseRelative() {
    // Gets the mouse position on screen
    Vector2 mouseInViewport = GetViewport().GetMousePosition();
    // Gets the player Position in ViewPort
    Vector2 playerInViewport = cam.UnprojectPosition(GlobalPosition);
    // Compares mouse position in viewport compared to playerPos in viewport
    Vector2 mouseRelative = playerInViewport - mouseInViewport;
    // return the comparison
    return mouseRelative;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
public
  override void _Process(double delta) {}

public
  override void _IntegrateForces(PhysicsDirectBodyState3D state) {
    base._IntegrateForces(state);
    Vector3 motionInput = GetMotion();
    // Mouse Functions
    this.LookAt(GetMouseRotation());

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

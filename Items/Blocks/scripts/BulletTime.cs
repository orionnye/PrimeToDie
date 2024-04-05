using Godot;
using System;

public partial class BulletTime : Item {
  // Timers and time scale
  float timer = 0f;
  float cooldownTimer = 0f;

  float duration = 2f;
  float cooldownDuration = 15f;

  float playerTimeScale = 0.85f;
  float worldTimeScale = 0.45f;

  public override void _Ready() {
    base._Ready();

    heldPosition = new Vector3(0, 0, 0);
    heldRotation = Vector3.Zero;

    activeMaterial = new StandardMaterial3D{ AlbedoColor = new Color(1f, 0f, 0f) };
    inactiveMaterial = new StandardMaterial3D{ AlbedoColor = new Color(1f, 1f, 1f) }; 
  }

  public override void Activate() {
    if (cooldownTimer > 0)
      return;

    active = true;
    GetNode("MeshInstance3D").Set("surface_material_override/0", activeMaterial);
    Engine.TimeScale = worldTimeScale;

    timer = duration;
    
  }

  public override void Deactivate() {
    active = false;
    GetNode("MeshInstance3D").Set("surface_material_override/0", inactiveMaterial);
    Engine.TimeScale = 1f;
    cooldownTimer = cooldownDuration;
  }

  public override void _Process(double delta) {
    if(timer > 0) {
      timer -= (float)delta;
    }
    
    if(cooldownTimer > 0) {
      cooldownTimer -= (float)delta;
    }

    if (timer <= 0 && active) {
      Deactivate();
    }
  }


}

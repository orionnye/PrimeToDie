using Godot;
using System;

public
partial class Floor : Node3D {
  [Export] PackedScene cube;

private
  Node3D CreateGrid(Vector3 dim, PackedScene gridCell) {
    Node3D host = new Node3D();
    for (int row = 0; row < dim.Y; row++) {
      for (int col = 0; col < dim.X; col++) {
        for (int stack = 0; stack < dim.Z; stack++) {
          // Instance the PackedScene
          Node3D gridCellInstance = (Node3D)gridCell.Instantiate();
          // Set its position based on the grid and spacing
          Vector3 position = new Vector3(col, stack, row);
          gridCellInstance.Position = position;

          // Add the instance as a child of this node
          host.AddChild(gridCellInstance);
        }
      }
    }
    return host;
  }
  // Called when the node enters the scene tree for the first time.
public
  override void _Ready() {
    Node3D floorHost = CreateGrid(new Vector3(25, 25, 1), cube);
    floorHost.Position = new Vector3(-12, 0, -12);
    AddChild(floorHost);
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
public
  override void _Process(double delta) {}
}

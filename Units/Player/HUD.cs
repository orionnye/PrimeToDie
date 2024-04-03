using Godot;
using System;

public partial class HUD : Control {

  private bool _inItemInteractRange = false;

  [Export] public RichTextLabel InteractLabel;

  public override void _Process(double delta) {
    base._Process(delta);

    InteractLabel.Visible = _inItemInteractRange;
  }

  public void OnItemWithinRange(bool insideRange) {
    _inItemInteractRange = insideRange;
  }

}

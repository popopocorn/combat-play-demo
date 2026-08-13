namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Godot;
using System;

public partial class Crouch : PlayerState
{
    [Export] private CollisionShape2D UpperCollision { get; set; }
    [Export] private Node2D UpperHandPosition { get; set; }
    [Export] private Node2D LowerHandPosition { get; set; }
    [Export] private Node2D HoldingItemPosition { get; set; }

    public override void _Ready()
    {
        HoldingItemPosition.GlobalPosition = UpperCollision.GlobalPosition;

        base._Ready();
    }


    public override void Enter()
    {
        UpperCollision.Disabled = true;
        HoldingItemPosition.GlobalPosition = LowerHandPosition.GlobalPosition;
    }

    public override void Exit()
    {
        UpperCollision.Disabled = false;
        HoldingItemPosition.GlobalPosition = UpperHandPosition.GlobalPosition;
    }

    public override void ApplyVelocity(double delta)
    {
        FSM.PlayerVelocity = Vector2.Down;

        FSM.FacingDirection = FSM.LastInputAxis_X;
    }
    
    public override void CheckIfSwitchState(double delta)
    {
        if (!FSM.Player.IsOnFloor())
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
        else if (FSM.InputAxis_Y != 1.0f)
        {
            if (FSM.InputAxis_X == 0.0f)
            {
                EmitSignalStateSwitchRequested(PlayerStateNames.Idle);
            }
            else
            {
                EmitSignalStateSwitchRequested(PlayerStateNames.Walk);
            }
        }
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputActionNames.Jump))
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Jump);
        }

        else if (@event.IsActionPressed(InputActionNames.Slash))
        {
            FSM.ActionManager.TryPickUpItems();
        }
        
    }
}

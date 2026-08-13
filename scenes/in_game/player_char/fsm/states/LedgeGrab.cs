namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Godot;
using System;

public partial class LedgeGrab : PlayerState
{
    [Export] private Timer LedgeGrabDuration { get; set; }
    [Export] private RayCast2D LedgeRay { get; set; }
    public override void Enter()
    {
        FSM.CurrentActionDirection = FSM.LastInputAxis_X;
        FSM.FacingDirection = FSM.CurrentActionDirection;

        LedgeGrabDuration.Start();
    }

    public override void Exit()
    {
        FSM.PrevActionDirection = FSM.CurrentActionDirection;
        FSM.CurrentActionDirection = 0.0f;

        LedgeGrabDuration.Stop();
    }

    public override void ApplyVelocity(double delta)
    {
        FSM.PlayerVelocity.X = FSM.CurrentActionDirection * FSM.Stats.WalkSpeed;

        if (FSM.Player.IsOnWallOnly())
        {
            FSM.PlayerVelocity.Y = FSM.Stats.LedgeClimbSpeed;
        }
        else
        {
            if (FSM.Player.IsOnFloor())
            {
                FSM.PlayerVelocity.Y = 1.0f;
            }
            else
            {
                FSM.PlayerVelocity.Y = -FSM.Stats.LedgeClimbSpeed;
            }
        }
    }


    public override void CheckIfSwitchState(double delta)
    {
        
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputActionNames.Jump))
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Jump);
        }
    }

    private void _on_ledge_grab_duration_timeout()
    {
        if (!FSM.Player.IsOnFloor())
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
        else
        {
            this.SwitchStateToIdleOrWalk();
        }
    }
}

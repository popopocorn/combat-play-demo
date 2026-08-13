namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Godot;
using System;

public partial class WallSlipper : PlayerState
{
    public override void Enter()
    {
        FSM.CurrentActionDirection = -FSM.Player.GetWallNormal().X;
        FSM.FacingDirection = -FSM.CurrentActionDirection;
    }

    public override void Exit()
    {
        FSM.PrevActionDirection = FSM.CurrentActionDirection;
        FSM.CurrentActionDirection = 0.0f;
    }

    public override void ApplyVelocity(double delta)
    {
        float targetSpeed;

        FSM.PlayerVelocity.X = FSM.CurrentActionDirection;

        if (FSM.InputAxis_X == 0.0f)
        {
            targetSpeed = FSM.Stats.WallSlipperSpeed;
        }
        else
        {
            targetSpeed = FSM.Stats.WallSlipperSpeed_Slow;
        }

        FSM.PlayerVelocity.Y = Mathf.MoveToward(FSM.PlayerVelocity.Y, targetSpeed, (float)delta / 6 * FSM.Player.GetGravity().Y);

        if (FSM.PlayerVelocity.Y > targetSpeed)
        {
            FSM.PlayerVelocity.Y = targetSpeed;
        }
    }

    public override void CheckIfSwitchState(double delta)
    {
        if (FSM.Player.IsOnFloor())
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Idle);
        }
        else if (FSM.InputAxis_X == FSM.FacingDirection)
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputActionNames.Jump))
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.WallJump);
        }
    }
}

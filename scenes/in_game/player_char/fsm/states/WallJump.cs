namespace Game.Player.FSM;

using Godot;
using System;

public partial class WallJump : PlayerState
{
    [Export] private Timer WallJumpDuration { get; set; }

    [Export] private Timer ForceWallJumpDuration { get; set; }

    bool IsForced { get; set; } = false;

    public override void Enter()
    {
        FSM.CurrentActionDirection = -FSM.PrevActionDirection;
        FSM.FacingDirection = FSM.CurrentActionDirection;

        ForceWallJumpDuration.Start();
        WallJumpDuration.Start();

        FSM.PlayerVelocity.X = FSM.Stats.WallJumpSpeed_X * FSM.CurrentActionDirection;
        FSM.PlayerVelocity.Y = FSM.Stats.WallJumpSpeed_Y;

        FSM.Player.Velocity = FSM.PlayerVelocity;

        IsForced = true;
    }

    public override void Exit()
    {
        FSM.PrevActionDirection = FSM.CurrentActionDirection;
        FSM.CurrentActionDirection = 0.0f;

        ForceWallJumpDuration.Stop();
        WallJumpDuration.Stop();

        IsForced = false;
    }

    public override void ApplyVelocity(double delta)
    {
        float targetAccel;
        
        if (FSM.InputAxis_X == -FSM.CurrentActionDirection && !IsForced)
        {
            targetAccel = FSM.Stats.WallJumpAccel_X_Fast;
        }
        else
        {
            targetAccel = FSM.Stats.WallJumpAccel_X;
        }

        FSM.PlayerVelocity.X = Mathf.MoveToward(FSM.PlayerVelocity.X, 0.0f, (float)delta * targetAccel);
        FSM.PlayerVelocity.Y = Mathf.MoveToward(FSM.PlayerVelocity.Y, FSM.Stats.MaxFallSpeed, (float)delta * FSM.Stats.WallJumpAccel_Y);

    }

    public override void CheckIfSwitchState(double delta)
    {
        if (FSM.Player.IsOnCeiling())
        {
            FSM.PlayerVelocity.Y = 0.0f;
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        
    }

    private void _on_force_wall_jump_duration_timeout()
    {
        IsForced = false;
    }

    private void _on_wall_jump_duration_timeout()
    {
        EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
    }

}

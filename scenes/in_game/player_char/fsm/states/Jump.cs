namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Game.Player.Actions;
using Godot;
using System;


public partial class Jump : PlayerState
{
    [Export] Timer JumpDuration { get; set; }

    public override void Enter()
    {
        FSM.ConsumeJumpBuffer();

        FSM.PlayerVelocity.Y = FSM.Stats.JumpSpeed;
        FSM.Player.Velocity = FSM.PlayerVelocity;

        JumpDuration.Start();
    }

    public override void Exit()
    {
        JumpDuration.Stop();
    }

    public override void ApplyVelocity(double delta)
    {
        FSM.PlayerVelocity.X = FSM.InputAxis_X * FSM.Stats.WalkSpeed;
        FSM.PlayerVelocity.Y = Mathf.MoveToward(FSM.PlayerVelocity.Y, FSM.Stats.MaxFallSpeed, (float)delta * FSM.Stats.JumpAccel_Y);

        FSM.FacingDirection = FSM.LastInputAxis_X;
    }
    
    public override void CheckIfSwitchState(double delta)
    {
        if (!Input.IsActionPressed(InputActionNames.Jump))
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
        else if (FSM.Player.IsOnCeiling())
        {
            FSM.PlayerVelocity.Y = 0.0f;
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionReleased(InputActionNames.Jump))
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
    }

    private void _on_jump_duration_timeout()
    {
        EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
    }
}

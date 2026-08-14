namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Godot;
using System;

public partial class SprintJump : PlayerState
{
    [Export]
    private float JumpDuration { get; set; }

    private Timer JumpDurationTimer { get; set; }

    public override void _Ready()
    {
        base._Ready();
        
        JumpDurationTimer = this.AddTimerToState(JumpDuration);
        JumpDurationTimer.Timeout += _OnJumpDurationTimeout;
    }


    public override void Enter()
    {
        FSM.ConsumeJumpBuffer();

        FSM.CurrentActionDirection = FSM.LastInputAxis_X;

        FSM.PlayerVelocity.X = FSM.Stats.SprintSpeed * FSM.CurrentActionDirection;
        FSM.PlayerVelocity.Y = FSM.Stats.JumpSpeed;
        FSM.Player.Velocity = FSM.PlayerVelocity;

        JumpDurationTimer.Start();
    }

    private void _OnJumpDurationTimeout()
    {
        EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
    }

    public override void _ExitTree()
    {
        JumpDurationTimer.Timeout -= _OnJumpDurationTimeout;
    }


    public override void Exit()
    {
        FSM.PrevActionDirection = FSM.CurrentActionDirection;
        FSM.CurrentActionDirection = 0.0f;
        
        JumpDurationTimer.Stop();
    }

    public override void ApplyVelocity(double delta)
    {
        float targetVelocity = FSM.CurrentActionDirection * FSM.Stats.WalkSpeed;

        FSM.PlayerVelocity.X = Mathf.MoveToward(FSM.PlayerVelocity.X, targetVelocity, (float)delta * FSM.Stats.SprintJumpAccel_X);
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

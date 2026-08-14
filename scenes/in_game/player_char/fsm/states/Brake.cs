namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Godot;
using System;

public partial class Brake : PlayerState
{
    [Export] private Timer BrakeDuration { get; set; }

    public override void Enter()
    {
        FSM.CurrentActionDirection = FSM.PrevActionDirection;
        BrakeDuration.Start();
    }

    public override void Exit()
    {
        FSM.PrevActionDirection = FSM.CurrentActionDirection;
        FSM.CurrentActionDirection = 0.0f;

        BrakeDuration.Stop();
    }

    public override void ApplyVelocity(double delta)
    {
        FSM.PlayerVelocity = Vector2.Zero;

        FSM.FacingDirection = FSM.CurrentActionDirection;
    }

    public override void CheckIfSwitchState(double delta)
    {
        if (!FSM.Player.IsOnFloor())
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Fall);
        }
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputActionNames.Jump))
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.Jump);
        }
    }

    private void _on_brake_duration_timeout()
    {
        this.SwitchStateToIdleOrWalk();
    }
}

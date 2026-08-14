namespace Game.Player.FSM;

using Godot;
using System;

public partial class Attack : PlayerState
{
    [Export]
    private float AttackDuration { get; set; }
    [Export]
    private float MoveDuration { get; set; }

    private Timer AttackDurationTimer { get; set; }
    private Timer MoveDurationTimer { get; set; }

    private bool IsMoving { get; set; } = false;

    public override void _Ready()
    {
        base._Ready();
        AttackDurationTimer = this.AddTimerToState(AttackDuration);
        AttackDurationTimer.Timeout += _OnAttackDurationTimeout;

        MoveDurationTimer = this.AddTimerToState(MoveDuration);
        MoveDurationTimer.Timeout += _OnMoveDurationTimeout;
    }

    private void _OnAttackDurationTimeout()
    {
        this.SwitchStateToIdleOrWalk();
    }

    private void _OnMoveDurationTimeout()
    {
        IsMoving = false;
    }

    public override void _ExitTree()
    {
        AttackDurationTimer.Timeout -= _OnAttackDurationTimeout;
        MoveDurationTimer.Timeout -= _OnMoveDurationTimeout;
    }



    public override void Enter()
    {
        FSM.CurrentActionDirection = FSM.LastInputAxis_X;

        AttackDurationTimer.Start();
        MoveDurationTimer.Start();

        IsMoving = true;
    }

    public override void Exit()
    {
        FSM.PrevActionDirection = FSM.CurrentActionDirection;
        FSM.CurrentActionDirection = 0.0f;

        AttackDurationTimer.Stop();
        MoveDurationTimer.Stop();
    }

    public override void ApplyVelocity(double delta)
    {
        if (IsMoving)
        {
            float targetVelocity = FSM.CurrentActionDirection * FSM.Stats.AttackVelocity_X;

            if (Mathf.Abs(FSM.PlayerVelocity.X) <= FSM.Stats.AttackVelocity_X)
            {
                FSM.PlayerVelocity.X = targetVelocity;
            }
            else
            {
                FSM.PlayerVelocity.X = Mathf.MoveToward(FSM.PlayerVelocity.X, targetVelocity, (float)delta * FSM.Stats.AttackAccel_X);
            }
        }
        else
        {
            FSM.PlayerVelocity.X = 0.0f;
        }

        FSM.PlayerVelocity.Y = 1.0f;
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

    }
}

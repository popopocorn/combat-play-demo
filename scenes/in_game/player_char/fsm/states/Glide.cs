namespace Game.Player.FSM;

using BitterCitrus.SRC.Core.BInput;
using Godot;
using System;

public partial class Glide : PlayerState
{
    [Export] private RayCast2D LedgeRay { get; set; }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void ApplyVelocity(double delta)
    {
        FSM.PlayerVelocity.X = FSM.InputAxis_X * FSM.Stats.WalkSpeed;
        FSM.PlayerVelocity.Y = Mathf.MoveToward(FSM.PlayerVelocity.Y, FSM.Stats.MaxFallSpeedDuringGlide, (float)delta * FSM.Player.GetGravity().Y);
        
        if (FSM.PlayerVelocity.Y > FSM.Stats.MaxFallSpeedDuringGlide)
        {
            FSM.PlayerVelocity.Y = FSM.Stats.MaxFallSpeedDuringGlide;
        }

        FSM.FacingDirection = FSM.LastInputAxis_X;
    }

    public override void CheckIfSwitchState(double delta)
    {
        if (FSM.Player.IsOnFloor())
        {
            this.SwitchStateToIdleOrWalk();
        }
        else if (LedgeRay.IsColliding() && LedgeRay.GetCollisionNormal().IsEqualApprox(Vector2.Up))
        {
            if (FSM.FacingDirection == FSM.InputAxis_X)
            {
                EmitSignalStateSwitchRequested(PlayerStateNames.LedgeGrab);
            }
        }
        else if (FSM.Player.IsOnWall() && FSM.Player.GetWallNormal().IsEqualApprox(new Vector2(-FSM.InputAxis_X, 0)) && FSM.PlayerVelocity.Y > 0)
        {
            EmitSignalStateSwitchRequested(PlayerStateNames.WallSlipper);
        }
        else if (!Input.IsActionPressed(InputActionNames.Jump))
        {
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
}

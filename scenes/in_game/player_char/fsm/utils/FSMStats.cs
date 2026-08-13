using Game.Player.FSM;

using Godot;
using System;

public partial class FSMStats : Resource
{
    [Export] public float WalkSpeed { get; private set; }
    [Export] public float JumpSpeed { get; private set; }
    [Export] public float JumpAccel { get; private set; }
    [Export] public float MaxFallSpeed { get; private set; }
    [Export] public float DashSpeed { get; private set; }
    [Export] public float SprintSpeed { get; private set; }

    [Export] public float DecelSpeed { get; private set; }
    [Export] public float MaxFallSpeedDuringSpin { get; private set; }


    [Export] public float LedgeClimbSpeed { get; private set; }
    [Export] public float WallSlipperSpeed { get; private set; }
    [Export] public float WallSlipperSpeed_Slow { get; private set; }
    [Export] public float WallJumpSpeed_X { get; private set; }
    [Export] public float WallJumpSpeed_Y { get; private set; }
    [Export] public float WallJumpAccel_X { get; private set; }
    [Export] public float WallJumpAccel_X_Fast { get; private set; }
    [Export] public float WallJumpAccel_Y { get; private set; }
}

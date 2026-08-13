namespace Game.Player;

using Godot;
using System;

public static class PlayerStateNames
{
    public static readonly StringName Idle = "Idle";
    public static readonly StringName Walk = "Walk";

    public static readonly StringName Crouch = "Crouch";
    public static readonly StringName Map = "Map";

    public static readonly StringName Dash = "Dash";
    public static readonly StringName Sprint = "Sprint";
    public static readonly StringName Brake = "Brake";

    public static readonly StringName Jump = "Jump";
    public static readonly StringName Fall = "Fall";
    public static readonly StringName SprintJump = "SprintJump";

    public static readonly StringName LedgeGrab = "LedgeGrab";

    public static readonly StringName WallSlipper = "WallSlipper";
    public static readonly StringName WallJump = "WallJump";

    public static readonly StringName Bash = "Bash";
    public static readonly StringName Spin = "Spin";
}

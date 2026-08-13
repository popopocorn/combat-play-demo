namespace Game.Player.Actions;

using BitterCitrus.SRC.Core.BInput;
using BitterCitrus.SRC.Entites;
using BitterCitrus.SRC.Handheld;
using Game.Player.FSM;
using Godot;
using System;


public partial class PlayerActionManager : Node2D
{
    [Export] PlayerChar Player { get; set; }
    [Export] Player_FSM FSM { get; set; }

    public bool IsHoldingItem { get; private set; } = false;
    public HandheldItem HoldingItem { get; private set; }



    // 공용 후딜레이 관리
    public bool CanExecuteAction { get; private set; } = true;
    [Export] Timer AfterCastDelayTimer { get; set; }


    [Export] HurtBox_Player Slash { get; set; }
    [Export] HurtBox_Player Bash { get; set; }
    [Export] HurtBox_Player Spin { get; set; }

    [Export] Area2D PickUpArea { get; set; }
    [Export] Node2D HoldingItemPosition { get; set; }
    [Export] Node2D NodeToFlip { get; set; }

    private HurtBox_Player CurrentAction;



    public override void _Ready()
    {
        Slash.AttackFinished += FinishAttack;
        Bash.AttackFinished += FinishAttack;
        Spin.AttackFinished += FinishAttack;

        CurrentAction = null;
    }

    private void FinishAttack()
    {
        CurrentAction = null;
    }

    

    public void TryPickUpItems()
    {
        if (!IsHoldingItem)
        {
            foreach (Node2D body in PickUpArea.GetOverlappingBodies())
            {
                if (body is HandheldItem item)
                {
                    PickUpItem(item);
                    break;
                }
            }
        }
        else
        {
            ReleaseItem();
        }

    }


    public void ExecuteAction_Right_Hand()
    {
        if (!CanExecuteAction) return;

        if (IsHoldingItem)
        {
            
        }
        else
        {
            if (Slash.CheckIfCanExecuteAction())
            {
                CurrentAction = Slash;
                Slash.Scale = new Vector2(FSM.FacingDirection, 1.0f);
                CurrentAction.ActivateHurtBox();
                StartAfterCastDelay(0.4f);
            }
        }
    }

    public void ExecuteAction_Left_Hand()
    {
        if (!CanExecuteAction) return;

        if (IsHoldingItem)
        {
            
        }
        else
        {
            if (Player.IsOnFloor())
            {
                if (Bash.CheckIfCanExecuteAction())
                {
                    CurrentAction = Bash;
                    Bash.Scale = new Vector2(FSM.FacingDirection, 1.0f);
                    CurrentAction.ActivateHurtBox();
                    StartAfterCastDelay(0.7f);
                    FSM.SwitchState(PlayerStateNames.Bash);
                }
            }
            else
            {
                if (Spin.CheckIfCanExecuteAction())
                {
                    CurrentAction = Spin;
                    Spin.Scale = new Vector2(FSM.FacingDirection, 1.0f);
                    CurrentAction.ActivateHurtBox();
                    StartAfterCastDelay(0.4f);
                    FSM.SwitchState(PlayerStateNames.Spin);
                }

            }
        }
    }

    public void ExecuteAction_Throw()
    {
        if (!CanExecuteAction) return;

        if (IsHoldingItem)
        {
            HoldingItem.ActivateAction_Throw(FSM.FacingDirection, FSM.InputAxis_Y);
            ReleaseItem();
        }
        else
        {
            
        }
    }

    public void ExecuteAction_Smash()
    {
        if (IsHoldingItem)
        {

        }
        else
        {
            
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentAction != null)
        {
            CurrentAction.ProcessHurtBox();
        }

        NodeToFlip.Scale = new Vector2(FSM.FacingDirection, 1.0f);
    }


    public void StartAfterCastDelay(float duration)
    {
        CanExecuteAction = false;
        AfterCastDelayTimer.Start(duration);
    }

    private void _on_after_cast_delay_timeout()
    {
        CanExecuteAction = true;
    }

    public void PickUpItem(HandheldItem item)
    {
        IsHoldingItem = true;
        HoldingItem = item;
        HoldingItem.Reparent(HoldingItemPosition, true);
        HoldingItem.PickUp();
        HoldingItem.GlobalPosition = HoldingItemPosition.GlobalPosition;
    }

    public void ReleaseItem()
    {
        IsHoldingItem = false;
        HoldingItem?.Release();
        HoldingItem.Reparent(GetTree().CurrentScene, true);
        HoldingItem = null;
    }
}
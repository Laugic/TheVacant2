using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheVacant.TheVacantCode.Gems;

public class GemOrb : CustomOrbModel
{
    public override decimal PassiveVal => ModifyOrbValue(Size);
    public override decimal EvokeVal => ModifyOrbValue(Size>0?GetAmount():0);
    public override Color DarkenedColor { get; }
    public int Size { get; set; } = 1;
    public override async Task AfterTurnStartOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext,  null);
    }
    
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        await TriggerEffect(choiceContext);
        await ReduceSize(choiceContext);
    }

    public virtual async Task TriggerEffect(PlayerChoiceContext choiceContext)
    {
        
    }

    private async Task ReduceSize(PlayerChoiceContext choiceContext)
    {
        Size--;
        if (Size <= 0)
            await Remove(choiceContext);
    }
    
    private async Task Remove(PlayerChoiceContext choiceContext)
    {
        Owner.PlayerCombatState?.OrbQueue.Remove(this);
        await OrbCmd.Channel(choiceContext, this, Owner);
        OrbCmd.RemoveSlots(Owner, 1);
    }

    protected virtual int GetAmount()
    {
        return 1;
    }
}
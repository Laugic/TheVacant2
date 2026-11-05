using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using TheVacant.TheVacantCode.Gems;

namespace TheVacant.TheVacantCode.Misc;

public class GemSingleton() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        /*
        MainFile.Logger.Info("GemSingleton trying to trigger slotted gems!");
        var gems = Satchel.Gems.Get(player.PlayerCombatState);
        if(gems is null)
            return;
        MainFile.Logger.Info("Gems are not null");
        var gemsToTrigger = gems.Where(gem => gem.isSlotted).ToList();
        foreach(var gem in gemsToTrigger)
            await gem.Trigger(player, choiceContext);*/
        //We do it this way because removing a gem should immediately slot in a new one,
        //and if we just checked the whole list for if a gem was slotted we would trigger like every gem I think
    }
}
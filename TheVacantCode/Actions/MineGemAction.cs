using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheVacant.TheVacantCode.Gems;

namespace TheVacant.TheVacantCode.Actions;

public static class MineGemAction
{
    public static async Task MineGem<T>(PlayerChoiceContext choiceContext, Player player, int size) where T : OrbModel
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        OrbModel gem = ModelDb.Orb<T>().ToMutable(0);
        ((GemOrb)gem).Size = size;
        await OrbCmd.AddSlots(player, 1);
        await OrbCmd.Channel(choiceContext, gem, player);
        return;
    }
}
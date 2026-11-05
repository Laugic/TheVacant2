using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheVacant.TheVacantCode.Gems;

namespace TheVacant.TheVacantCode.Actions;

public static class ChipGemAction
{
    public static async Task Chip(PlayerChoiceContext choiceContext, Player player, OrbModel specificGem)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        if (player.PlayerCombatState.OrbQueue.Orbs.Contains(specificGem))
            await ((GemOrb)specificGem).Passive(choiceContext, null);
    }

    public static async Task Chip(PlayerChoiceContext choiceContext, Player player, int numTimes)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        int curTimes = 1;
        while (curTimes <= numTimes && player.PlayerCombatState.OrbQueue.Orbs.Any(orb => orb is GemOrb))
        {
            GemOrb gem = (GemOrb)GetGemAtIndex(player, 1);
            if (gem != null)
            {
                await gem.Passive(choiceContext, null);
                curTimes++;
            }
            else
                return;
        }
    }

    public static async Task Chip(PlayerChoiceContext choiceContext, int numGems, Player player)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        int curTimes = 1;
        while (curTimes <= numGems && player.PlayerCombatState.OrbQueue.Orbs.Any(orb => orb is GemOrb))
        {
            GemOrb gem = (GemOrb)GetGemAtIndex(player, curTimes);
            if (gem != null)
            {
                await gem.Passive(choiceContext, null);
                curTimes++;
            }
            else
                return;
        }
    }

    private static OrbModel GetGemAtIndex(Player player, int index)
    {
        if (player.PlayerCombatState.OrbQueue.Orbs.Count(orb => orb is GemOrb) >= index)
        {
            int curGemIndex = 0;
            for (int i = 0; i < index && i < player.PlayerCombatState.OrbQueue.Orbs.Count; i++)
            {
                if (player.PlayerCombatState.OrbQueue.Orbs[i] is GemOrb)
                {
                    curGemIndex++;
                    if (curGemIndex == index)
                        return player.PlayerCombatState.OrbQueue.Orbs[i];
                }
            }
        }
        return null;
    }
}
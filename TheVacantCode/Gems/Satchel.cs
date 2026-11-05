/*using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Random;

namespace TheVacant.TheVacantCode.Gems;

public static class Satchel
{
    public static readonly SpireField<PlayerCombatState, List<Gem>> Gems = new(() => new List<Gem>());
    public static readonly SpireField<PlayerCombatState, int> Slots = new(() => 2);

    public static void TryFillGemSlots(PlayerCombatState combatState)
    {
        var gems = Gems[combatState];
        var gemsToSlot = gems.Where(gem => !gem.isSlotted).ToList();
        if (gemsToSlot.Count == 0)
            return;
        var slottedGems = gems.Where(gem => gem.isSlotted).ToList();
        while (slottedGems.Count < Slots[combatState] && gemsToSlot.Count > 0)
        {
            var gem = gemsToSlot.First();
            gem.isSlotted = true;
            MainFile.Logger.Info("Slotted a " + gem.GetType().Name + "!");
            slottedGems.Add(gem);
            gemsToSlot.Remove(gem);
        }
    }
}*/
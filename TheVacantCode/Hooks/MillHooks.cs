using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheVacant.TheVacantCode.Cards;
using TheVacant.TheVacantCode.SpireFields;

namespace TheVacant.TheVacantCode.Hooks;

public class MillHooks
{
    public static async Task AfterCardMilled(
        CombatState combatState,
        PlayerChoiceContext choiceContext,
        CardModel card,
        Player player)
    {
        AbstractModel model;
        CardPile hand = PileType.Hand.GetPile(player);
        if (MillFields.Ricochet[card])
        {
            await CardPileCmd.Add(card, hand);
            if(card is TheVacantCard vCard)
                vCard.OnRicochet();
        }
    }
}
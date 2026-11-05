using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheVacant.TheVacantCode.Cards;
using TheVacant.TheVacantCode.Hooks;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.SpireFields;

namespace TheVacant.TheVacantCode.Actions;

public class ReturnAction
{
  public static async Task<IEnumerable<CardModel>> Return(
    PlayerChoiceContext choiceContext,
    Decimal count,
    Player player)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return  Array.Empty<CardModel>();
    List<CardModel> result = new List<CardModel>();
    CardPile discard = PileType.Discard.GetPile(player);
    CardPile hand = PileType.Hand.GetPile(player);
    int returnAmount = count > 0M ? (int) Math.Ceiling(count) : 0;
    if (returnAmount == 0)
      return  result;
    for (int i = 0; i < returnAmount; ++i)
    {
        CardModel card = discard.Cards.LastOrDefault();
        if (card != null)
        {
          result.Add(card);
          if (hand.Cards.Count < 10)
            await CardPileCmd.Add(card, hand);
          NDebugAudioManager.Instance?.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
          card = (CardModel) null;
        }
        else
          break;
    }
    return result;
  }
}
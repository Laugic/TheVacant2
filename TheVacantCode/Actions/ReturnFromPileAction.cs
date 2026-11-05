using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace TheVacant.TheVacantCode.Actions;

public static class ReturnFromPileAction
{
    public enum ReturnType
    {
        TOP,
        RANDOM,
        CHOOSE
    }
    public static async Task<IEnumerable<CardModel>> ReturnFromPile(
        PlayerChoiceContext choiceContext,
        CardModel cardPlayed,
        Player player,
        CardPile pile,
        Decimal count,
        ReturnType  returnType,
        LocString selectionScreenPrompt = null)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return  Array.Empty<CardModel>();
        List<CardModel> result = new List<CardModel>();
        CardPile hand = PileType.Hand.GetPile(player);
        int returnAmount = count > 0M ? (int) Math.Ceiling(count) : 0;
        if (returnAmount == 0)
            return  result;
        CardModel card = pile.Cards.LastOrDefault();
        switch (returnType)
        {
            case ReturnType.TOP:
                for (int i = 0; i < returnAmount; ++i)
                {
                    card = pile.Cards.LastOrDefault();
                    if (card != null)
                    {
                        result.Add(card);
                        if (hand.Cards.Count < 10)
                            await CardPileCmd.Add(card, hand);
                        NDebugAudioManager.Instance?.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
                    }
                    else
                        break;
                }
                break;
            case ReturnType.RANDOM:
                for (int i = 0; i < returnAmount; ++i)
                {
                    card = pile.Cards[player.RunState.Rng.CombatCardSelection.NextInt(0, pile.Cards.Count)];
                    if (card != null)
                    {
                        result.Add(card);
                        if (hand.Cards.Count < 10)
                            await CardPileCmd.Add(card, hand);
                        NDebugAudioManager.Instance?.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
                    }
                    else
                        break;
                }
                break;
            case ReturnType.CHOOSE:
                //TODO: CONTINUE THIS TO MAKE IT WORK
                CardSelectorPrefs prefs = new CardSelectorPrefs(selectionScreenPrompt, returnAmount);
                var cards = await CardSelectCmd.FromCombatPile(choiceContext, pile, player, prefs);
                foreach (var c in cards)
                {
                    result.Add(c);
                    if (hand.Cards.Count < 10)
                        await CardPileCmd.Add(c, hand);
                }
                //CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Bottom);
                break;
        }
        return result;
    }
}
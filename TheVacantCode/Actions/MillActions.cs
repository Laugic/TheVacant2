using System.Reflection;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheVacant.TheVacantCode.Cards;
using TheVacant.TheVacantCode.CombatEntries;
using TheVacant.TheVacantCode.Hooks;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.Powers;
using TheVacant.TheVacantCode.Relics;
using TheVacant.TheVacantCode.SpireFields;

namespace TheVacant.TheVacantCode.Actions;

public class MillActions
{
  public static async Task<IEnumerable<CardModel>> Mill(
    PlayerChoiceContext choiceContext,
    Decimal count,
    Player player, ICombatState combatState, Func<CardModel, bool> condition = null)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return Array.Empty<CardModel>();
    List<CardModel> result = new List<CardModel>();
    CardPile discard = PileType.Discard.GetPile(player);
    CardPile hand = PileType.Hand.GetPile(player);
    CardPile drawPile = PileType.Draw.GetPile(player);
    int millAmount = (int)count;
/*
    if (player.HasPower<SpiritPower>())
    {
      foreach (var p in player.Creature.Powers)
      {
        if (p is SpiritPower s)
          millAmount += s.Amount;
      }
    }
    */
    if (millAmount == 0)
      return result;
    
    foreach (var power in player.Creature.Powers)
    {
      if(power is OnMillPower p)
        p.WhenMill();
    }
/*
    //Tracking cards milled
    var history = CombatManager.Instance.History;

    MethodInfo addMethod = history.GetType().GetMethod(
      "Add",
      BindingFlags.NonPublic | BindingFlags.Instance
    );
    addMethod.Invoke(history, new object[] {(new MilledEntry(player.Creature, millAmount, combatState.RoundNumber, combatState.CurrentSide, CombatManager.Instance.History))});
    //end tracking
    */

    List<CardModel> cardsToBeMilled = new List<CardModel>();
    List<CardModel> specialRicochets = new List<CardModel>();

    if (millAmount > 0)
    {
      for (int i = 0; i < millAmount && i < drawPile.Cards.Count; ++i)
        cardsToBeMilled.Add(drawPile.Cards[i]);
    }
    else
    {
      cardsToBeMilled.AddRange(drawPile.Cards);
    }

    if (cardsToBeMilled.Count == 0)
      return result;
    
    foreach (RelicModel relic in player.Relics)
    {
      if (relic is OnMillRelic r)
        specialRicochets.AddRange(r.UnconditionalRicochets(cardsToBeMilled, specialRicochets));
    }

    foreach (var power in player.Creature.Powers)
    {
      if(power is OnMillPower p)
        specialRicochets.AddRange(p.UnconditionalRicochets(cardsToBeMilled, specialRicochets));
    }
    
    foreach (RelicModel relic in player.Relics)
    {
      if (relic is OnMillRelic r)
        specialRicochets.AddRange(r.GetExtraRicochets(cardsToBeMilled, specialRicochets));
    }
    
    foreach (var power in player.Creature.Powers)
    {
      if(power is OnMillPower p)
        specialRicochets.AddRange(p.GetExtraRicochets(cardsToBeMilled, specialRicochets));
    }
    
    foreach(CardModel card in  cardsToBeMilled)
    {
        if (card != null)
        {
          result.Add(card);
          if ((card.Keywords.Contains(TheVacantKeywords.Ricochet)  || card is Ricochet || specialRicochets.Contains(card)) && hand.Cards.Count < 10)
          {
            await CardPileCmd.Add(card, hand);
            if(card is TheVacantCard vCard)
              vCard.OnRicochet();
            foreach (RelicModel relic in player.Relics)
            {
              if (relic is OnMillRelic r)
                r.OnRicochet(card);
            }
    
            foreach (var power in player.Creature.Powers)
            {
              if(power is OnMillPower p)
                p.OnRicochet(card);
            }
          }
          else if (condition!= null && condition(card))
          {
            if (hand.Cards.Count < 10)
            {
              //TODO: Add ricochet effects here?
              await CardPileCmd.Add(card, hand);
              return result;
            }
          }
          else
          {
            await CardPileCmd.Add(card, discard);
          }
          foreach (RelicModel relic in player.Relics)
          {
            if (relic is OnMillRelic r)
              r.OnMill(card);
          }
    
          foreach (var power in player.Creature.Powers)
          {
            if(power is OnMillPower p)
              p.OnMill(card);
          }

          CardCmd.Preview(card, .25f, CardPreviewStyle.MessyLayout);
          NDebugAudioManager.Instance?.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
        }
        else
          break;
    }
    return result;
  }
}
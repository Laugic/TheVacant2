using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace TheVacant.TheVacantCode.Relics;

public class BrassGoblet : TheVacantRelic, OnMillRelic
{
    public List<CardModel> GetExtraRicochets(List<CardModel> cards)
    {
        if (cards == null || cards.Count == 0)
            return cards;
        var lowestCostCard = cards.FirstOrDefault();
        foreach (var card in cards)
        {
            if(card.EnergyCost.GetWithModifiers(CostModifiers.Local) < lowestCostCard.EnergyCost.GetWithModifiers(CostModifiers.Local))
                lowestCostCard = card;
        }

        List<CardModel> ricochet = new List<CardModel>();
        ricochet.Add(lowestCostCard);
        return ricochet;
    }

    public override RelicRarity Rarity => RelicRarity.Starter;
}
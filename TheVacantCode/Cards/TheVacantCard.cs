using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheVacant.TheVacantCode.Character;
using TheVacant.TheVacantCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using TheVacant.TheVacantCode.Powers;

namespace TheVacant.TheVacantCode.Cards;

[Pool(typeof(TheVacantCardPool))]
public abstract class TheVacantCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    //public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    
    public virtual void OnRicochet(){}

    public static int GetBonusFromSpirit(Player player, int amount)
    {
        int result = amount;
        foreach (var p in player.Creature.Powers)
        {
            if(p is SpiritPower s)
                result += s.Amount;
        }
        //TODO: Add relics here that affect mill amount
        return result;
    }
}
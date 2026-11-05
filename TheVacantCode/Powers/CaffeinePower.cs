using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheVacant.TheVacantCode.Powers;


public class CaffeinePower : TheVacantPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => new Data();
    
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {};

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        CaffeinePower power = this;
        Data internalData = power.GetInternalData<Data>();
        internalData.exhaustedBefore = cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust);
        if(internalData.exhaustedBefore)
            cardPlay.Card.RemoveKeyword(CardKeyword.Exhaust);
        return base.BeforeCardPlayed(cardPlay);
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        return base.AfterCombatEnd(room);
    }

    public override Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CaffeinePower power = this;
        Data internalData = power.GetInternalData<Data>();
        if(internalData.exhaustedBefore)
            cardPlay.Card.AddKeyword(CardKeyword.Exhaust);
        return base.AfterCardPlayedLate(choiceContext, cardPlay);
    }

    private class Data
    {
        public bool exhaustedBefore = false;
    }
}
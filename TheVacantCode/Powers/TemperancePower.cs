using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheVacant.TheVacantCode.Powers;


public class TemperancePower : TheVacantPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => new Data();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.Static(StaticHoverTip.Block)};

    public override Task BeforeBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (creature != Owner || cardSource?.Owner.Creature != Owner)
            return Task.CompletedTask;
        Data internalData = GetInternalData<Data>();
        internalData.amountWhenBlockStarted = Amount;
        return Task.CompletedTask;
    }
    
    public override Decimal ModifyBlockAdditive(
        Creature target,
        Decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null)
        {
            if (cardSource.Owner.Creature != Owner)
                return 0M;
        }
        else if (Owner != target)
            return 0M;
        return !props.IsPoweredCardOrMonsterMoveBlock() ? 0M : Amount;
    }

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        TemperancePower power = this;
        Data internalData = power.GetInternalData<Data>();
        int num = await PowerCmd.ModifyAmount(new BlockingPlayerChoiceContext(), power, -internalData.amountWhenBlockStarted, null, null);
    }
    
    private class Data
    {
        public int amountWhenBlockStarted;
    }
}
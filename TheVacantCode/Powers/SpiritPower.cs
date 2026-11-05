using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheVacant.TheVacantCode.Powers;


public class SpiritPower : TheVacantPower, OnMillPower
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
        internalData.amountWhenCardPlayed = Amount;
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
    
    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != this.Owner || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        Data internalData = this.GetInternalData<Data>();
        if (internalData.commandToModify != null || command.ModelSource != null && !(command.ModelSource is CardModel) || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        internalData.commandToModify = command;
        internalData.amountWhenCardPlayed = Amount;
        return Task.CompletedTask;
    }

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (Owner != dealer || !props.IsPoweredAttack())
            return 0M;
        Data internalData = GetInternalData<Data>();
        return internalData.commandToModify != null && cardSource != null && cardSource != internalData.commandToModify.ModelSource || internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer ? 0M : Amount;
    }
/*
    public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target,
        CardModel? cardSource)
    {
        if (power is GroovePower && giver == Owner)
        {
            Data internalData = GetInternalData<Data>();
            internalData.amountWhenCardPlayed = Amount;
            return Amount;
        }
        return base.ModifyPowerAmountGivenAdditive(power, giver, amount, target, cardSource);
    }*/
/*
    public void WhenMill()
    {
        Data internalData = GetInternalData<Data>();
        internalData.amountWhenCardPlayed = Amount;
    }

    public int ModiifyMillAmount(Player player, int amount)
    {
        if (player == Owner.Player)
        {
            Data internalData = GetInternalData<Data>();
            return amount + Amount;
        }
        return amount;
    }*/

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Data internalData = GetInternalData<Data>();
        if (cardPlay.Card.Type != CardType.Attack && !cardPlay.Card.GainsBlock)// &&
            //!cardPlay.Card.DynamicVars.ContainsKey("GroovePower") && !cardPlay.Card.DynamicVars.ContainsKey("Mill"))
            return;
        int num = await PowerCmd.ModifyAmount(choiceContext, this, -internalData.amountWhenCardPlayed, null, null);
    }

    private class Data
    {
        public int amountWhenCardPlayed;
        public AttackCommand? commandToModify;
    }
}
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
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


public class GroovePower : TheVacantPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.Static(StaticHoverTip.Block)};

    //public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    //{
    //    if (player != Owner.Player)
    //        return;
    //    await PowerCmd.Remove( this);
    //}

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        await Trigger(choiceContext, side);
    }

    public async Task Trigger(PlayerChoiceContext choiceContext, CombatSide side, bool AllEnemies = false)
    {
        if (side != Owner.Side)
            return;
        Flash();
        for (int i = 0; i < GetNumHits(); i++)
        {
            if(AllEnemies)
                await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner);
            else
            {
                Creature target = Owner.Player.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                if (target == null)
                    return;
                await CreatureCmd.Damage(choiceContext, target, Amount, ValueProp.Unpowered, Owner);
            }
        }
    }

    private int GetNumHits()
    {
        return 1;//TODO: Add checks here for powers that increase the number of triggers
    }
}
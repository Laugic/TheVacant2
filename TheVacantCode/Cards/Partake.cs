using BaseLib.Utils;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheVacant.TheVacantCode.Actions;
using TheVacant.TheVacantCode.Cards;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.Powers;
using TheVacant.TheVacantCode.SpireFields;

namespace TheVacant.TheVacantCode.Cards;


[UsedImplicitly]
public class Partake() : TheVacantCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new PowerVar<SpiritPower>(7),
            new PowerVar<TaintedPower>(2)
        ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.FromPower<SpiritPower>(), HoverTipFactory.FromPower<TaintedPower>()};
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<SpiritPower>(choiceContext, Owner.Creature, DynamicVars["SpiritPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<TaintedPower>(choiceContext, Owner.Creature, DynamicVars["TaintedPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SpiritPower"].UpgradeValueBy(3M);
    }
}
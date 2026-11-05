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
public class Corporeate() : TheVacantCard(2, CardType.Skill, CardRarity.Basic, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new BlockVar(10m, ValueProp.Move),
            new MillVar(5)
            //new PowerVar<GroovePower>(5)
        ]);

    //protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.FromPower<GroovePower>(), HoverTipFactory.Static(TheVacantKeywords.Return)};
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CommonActions.CardBlock(this, play);
        await MillActions.Mill(choiceContext, DynamicVars["Mill"].BaseValue, Owner, CombatState);
        //await PowerCmd.Apply<GroovePower>(choiceContext, Owner.Creature, DynamicVars["GroovePower"].BaseValue, Owner.Creature, this);
        //await ReturnAction.Return(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4M);
        DynamicVars["Mill"].UpgradeValueBy(2M);
    }
}
using BaseLib.Extensions;
using BaseLib.Utils;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
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
public class Riff() : TheVacantCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new BlockVar(8M, ValueProp.Move)
        ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.FromPower<GroovePower>()};
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (Owner.HasPower<GroovePower>())
        {
            await Owner.Creature.GetPower<GroovePower>()!.Trigger(choiceContext, CombatSide.Player);
            if(IsUpgraded)
                await Owner.Creature.GetPower<GroovePower>()!.Trigger(choiceContext, CombatSide.Player);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(1m);
    }
}
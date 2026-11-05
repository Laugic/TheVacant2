using BaseLib.Utils;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
public class StrikeAChord() : TheVacantCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new DamageVar(8, ValueProp.Move)
        ]);
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        GroovePower groovePower = await PowerCmd.Apply<GroovePower>(choiceContext, cardPlay.Target, (await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext)).Results.SelectMany((r => r)).Sum((r => r.TotalDamage)), Owner.Creature,  this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
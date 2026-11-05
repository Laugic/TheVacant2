using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheVacant.TheVacantCode.Actions;
using TheVacant.TheVacantCode.Cards;
using TheVacant.TheVacantCode.Gems;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.Powers;

namespace TheVacant.TheVacantCode.Cards;


[UsedImplicitly]
public class SymphonicBlast() : TheVacantCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new CalculationBaseVar(0M),
            new ExtraDamageVar(1M),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState.DiscardPile.Cards.Count()))
        ]);
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(TheVacantKeywords.Ricochet);
    }
}
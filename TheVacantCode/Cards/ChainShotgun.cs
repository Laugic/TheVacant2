using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
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
public class ChainShotgun() : TheVacantCard(2, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    //public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>() { TheVacantKeywords.Ricochet };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new DamageVar(6M, ValueProp.Move),
            new RepeatVar(3)
        ]);
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this, cardPlay).TargetingRandomOpponents(CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        CardCmd.Downgrade(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}
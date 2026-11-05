using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheVacant.TheVacantCode.Actions;
using TheVacant.TheVacantCode.Cards;
using TheVacant.TheVacantCode.Gems;
using TheVacant.TheVacantCode.Gems.UI;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.Powers;

namespace TheVacant.TheVacantCode.Cards;


[UsedImplicitly]
public class EmeraldSplash() : TheVacantCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>() { CardKeyword.Exhaust };

    private const string GemsVar = nameof(Gems.Gems.Emerald);
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new DamageVar(3m, ValueProp.Move),
            new DynamicVar(nameof(Gems.Gems.Emerald), 1)
        ]);


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [Gems.Gems.Emerald.Tip()];

    //protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.FromOrb<EmeraldGem>() };
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, play);
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
        
        Pouch.Add(Gems.Gems.Emerald, DynamicVars[GemsVar].IntValue);
        //await MineGemAction.MineGem<EmeraldGem>(choiceContext, Owner, (int)DynamicVars["Size"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Size"].UpgradeValueBy(1M);
    }
}
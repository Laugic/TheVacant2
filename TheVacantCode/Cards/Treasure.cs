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
using TheVacant.TheVacantCode.Gems;
using TheVacant.TheVacantCode.Gems.UI;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.Powers;
using TheVacant.TheVacantCode.SpireFields;

namespace TheVacant.TheVacantCode.Cards;


[UsedImplicitly]
public class Treasure() : TheVacantCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>() { CardKeyword.Exhaust };

    //protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.FromPower<GroovePower>(), HoverTipFactory.Static(TheVacantKeywords.Return)};
    
    private const string GemsVar = nameof(Gems.Gems.Emerald);
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new DynamicVar(nameof(Gems.Gems.Emerald), 1)
        ]);
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (Owner.PlayerCombatState == null)
            return;
        //var gems =  Satchel.Gems.Get(Owner.PlayerCombatState) ?? new List<Gem>();
        //gems.Add(new EmeraldGem());
        //Satchel.Gems.Set(Owner.PlayerCombatState, gems);
        Pouch.Add(Gems.Gems.Emerald, DynamicVars[GemsVar].IntValue);
        //Satchel.TryFillGemSlots(Owner.PlayerCombatState);
        //await MillActions.Mill(choiceContext, DynamicVars["Mill"].BaseValue, Owner, CombatState);
        //await PowerCmd.Apply<GroovePower>(choiceContext, Owner.Creature, DynamicVars["GroovePower"].BaseValue, Owner.Creature, this);
        //await ReturnAction.Return(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        
    }

    protected override void OnUpgrade()
    {
        //TODO: Remove Exhaust and then Downgrade
    }
}
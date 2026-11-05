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
public class CoffeePot() : TheVacantCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new DamageVar(8, ValueProp.Move)
        ]);
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await ReturnFromPileAction.ReturnFromPile(choiceContext, this, Owner, PileType.Exhaust.GetPile(Owner), 1,
            ReturnFromPileAction.ReturnType.RANDOM);

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
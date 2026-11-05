using BaseLib.Extensions;
using BaseLib.Utils;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
using TheVacant.TheVacantCode.CombatEntries;
using TheVacant.TheVacantCode.Keywords;
using TheVacant.TheVacantCode.Powers;
using TheVacant.TheVacantCode.SpireFields;

namespace TheVacant.TheVacantCode.Cards;

[UsedImplicitly]
public class Strum() : TheVacantCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new MillVar(4),
            new CalculationBaseVar(0),
            new CalculationExtraVar(1),
            new CalculatedVar("MilledSoFar").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => CombatManager.Instance.History.Entries.OfType<MilledEntry>().Count( (e => e.Actor == card.Owner.Creature))))
        ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>() {HoverTipFactory.FromPower<GroovePower>()};
    
    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await MillActions.Mill(choiceContext, DynamicVars["Mill"].BaseValue, Owner, CombatState);
        await Cmd.Wait(.2f);
        //await PowerCmd.Apply<GroovePower>(Owner.Creature, DynamicVars["MilledSoFar"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Mill"].UpgradeValueBy(2M);
    }
}
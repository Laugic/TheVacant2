using BaseLib.Utils;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheVacant.TheVacantCode.Cards;

namespace TheVacant.TheVacantCode.Cards;


[UsedImplicitly]
public class Deflection() : TheVacantCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>([
            new BlockVar(8M, ValueProp.Move),
            new BlockVar("BlockNextTurn", 8M, ValueProp.Move)
        ]);

    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Deflection cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        BlockVar dynamicVar = (BlockVar) cardSource.DynamicVars["BlockNextTurn"];
        Decimal blockNextTurnAmount = Hook.ModifyBlock(cardSource.CombatState, cardSource.Owner.Creature, dynamicVar.BaseValue, dynamicVar.Props, (CardModel) cardSource, cardPlay, out IEnumerable<AbstractModel> _);
        Decimal num = await CreatureCmd.GainBlock(cardSource.Owner.Creature, cardSource.DynamicVars.Block, cardPlay);
        BlockNextTurnPower blockNextTurnPower = await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, cardSource.Owner.Creature, blockNextTurnAmount, cardSource.Owner.Creature, (CardModel) cardSource);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(2m);
        DynamicVars["BlockNextTurn"].UpgradeValueBy(2M);
    }
}
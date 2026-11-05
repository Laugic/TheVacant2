using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace TheVacant.TheVacantCode.CombatEntries;

public class MilledEntry(
    Creature actor,
    int amountMilled,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history, IEnumerable<Player> players)
    : CombatHistoryEntry(actor, roundNumber, currentSide, history, players)
{
    public readonly int AmountMilled = amountMilled;
    
    public override string Description =>
        $"{Actor.Player?.Character.Id.Entry} milled {AmountMilled} cards";
}
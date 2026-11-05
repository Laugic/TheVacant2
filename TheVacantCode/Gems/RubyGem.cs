using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheVacant.TheVacantCode.Gems;

public class RubyGem : GemOrb
{
    protected override int GetAmount()
    {
        return 5;
    }

    public override async Task TriggerEffect(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, GetAmount(), Owner.Creature, null);
    }
    public override string? CustomSpritePath => "res://scenes/orbs/orb_visuals/lightning_orb.tscn";
    public override string? CustomIconPath   => "res://images/orbs/lightning_orb.png";
}
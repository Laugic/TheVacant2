using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheVacant.TheVacantCode.Gems;

public class OpalGem : GemOrb
{
    protected override int GetAmount()
    {
        return 1;
    }

    public override async Task TriggerEffect(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(GetAmount(), Owner);
    }
    public override string? CustomSpritePath => "res://scenes/orbs/orb_visuals/lightning_orb.tscn";
    public override string? CustomIconPath   => "res://images/orbs/lightning_orb.png";
    
}
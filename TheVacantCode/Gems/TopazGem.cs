using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheVacant.TheVacantCode.Gems;

public class TopazGem : GemOrb
{
    protected override int GetAmount()
    {
        return 1;
    }

    public override async Task TriggerEffect(PlayerChoiceContext choiceContext)
    {
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsUpgradable))
            CardCmd.Upgrade(card);
        await Cmd.Wait(.2f);
    }
    public override string? CustomSpritePath => "res://scenes/orbs/orb_visuals/lightning_orb.tscn";
    public override string? CustomIconPath   => "res://images/orbs/lightning_orb.png";
}
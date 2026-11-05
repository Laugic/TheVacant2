/*using Godot;
using GodotPlugins.Game;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheVacant.TheVacantCode.Gems;

public class EmeraldGem : Gem
{
    public EmeraldGem()
    {
        DrawNode = ResourceLoader.Load<PackedScene>("res://TheVacant/scenes/gems/emerald.tscn").Instantiate();
        MainFile.Logger.Info("Tried to make an Emerald!");
    }
    protected override async Task TriggerEffect(Player player, PlayerChoiceContext choiceContext)
    {
        MainFile.Logger.Info("Tried to trigger Emerald!");
        await CardPileCmd.Draw(choiceContext, 1, player);
    }
}*/
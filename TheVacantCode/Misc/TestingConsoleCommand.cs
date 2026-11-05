using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;

namespace TheVacant.TheVacantCode.Misc;

public class TestingConsoleCommand : AbstractConsoleCmd
{
    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (issuingPlayer == null || !RunManager.Instance.IsInProgress)
            return new CmdResult(false, "A run does not appear to be in progress, whoopsie!");

        TaskHelper.RunSafely(TestingConsoleCommand.EnableTesting(issuingPlayer));
        
        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
        defaultInterpolatedStringHandler.AppendLiteral("mangotastic");
        return new CmdResult(true, defaultInterpolatedStringHandler.ToStringAndClear());
    }

    public override string CmdName => "testing";
    public override string Args => "";
    public override string Description => "mango time";
    public override bool IsNetworked => false;

    private static async Task EnableTesting(Player player)
    {
        Creature playerCreature = player.Creature;
        //StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(playerCreature, -9999M, playerCreature, (CardModel) null);
        //BufferPower bufferPower = await PowerCmd.Apply<BufferPower>(playerCreature, 9999M, playerCreature, (CardModel) null);
        //RegenPower regenPower = await PowerCmd.Apply<RegenPower>(playerCreature, 9999M, playerCreature, (CardModel) null);
        
        for (int i = 0; i < 15; i++)
            await RelicCmd.Obtain<Mango>(player);
        playerCreature = (Creature) null;
    }
}
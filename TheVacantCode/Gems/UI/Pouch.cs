using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace TheVacant.TheVacantCode.Gems.UI;


public static class Pouch
{
    public const int StackSize = 64;

    [SavedProperty]
    private static int[] GemCounts { get; set; } = new int[Gem.All.Length];

    //public event Action? Changed;

    //public int this[Gems gem] => GemCounts[(int)gem];

    public static int Total => GemCounts.Sum();

    //public static Pouch Of(Player player) => player.GetRelic<Pouch>()!;

    public static IEnumerable<(Gems Gem, int Count)> Stacks()
    {
        foreach (var gem in Gem.All)
            for (var left = GemCounts[(int)gem]; left > 0; left -= StackSize)
                yield return (gem, Math.Min(left, StackSize));
    }

    public static void Add(Gems gem, int amount) => Set(gem, GemCounts[(int)gem] + amount);

    public static void Remove(Gems gem, int amount) => Set(gem, Math.Max(0, GemCounts[(int)gem] - amount));

    public static int TakeAll()
    {
        var total = Total;
        Store(new int[Gem.All.Length]);
        return total;
    }

    private static void Set(Gems gem, int count)
    {
        // copy: ToMutable shares the canonical array
        var counts = GemCounts.ToArray();
        counts[(int)gem] = count;
        Store(counts);
    }

    private static void Store(int[] counts)
    {
        //AssertMutable();
        GemCounts = counts;
        //Changed?.Invoke();
    }
}

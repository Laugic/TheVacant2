using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheVacant.TheVacantCode.Gems;

public enum Gems { Ruby, Sapphire, Emerald, Opal, Amethyst, Topaz, Onyx, Diamond }

public static class Gem
{
    public static readonly Gems[] All = Enum.GetValues<Gems>();

    [CustomEnum] public static CardTag Tag;
    [CustomEnum] public static StaticHoverTip Ruby;
    [CustomEnum] public static StaticHoverTip Sapphire;
    [CustomEnum] public static StaticHoverTip Emerald;

    public static IHoverTip Tip(this Gems gem) => HoverTipFactory.Static(gem switch
    {
        Gems.Ruby => Ruby,
        Gems.Sapphire => Sapphire,
        Gems.Emerald => Emerald,
        _ => throw new ArgumentOutOfRangeException(nameof(gem)),
    });

    public static Texture2D Icon(this Gems gem) =>
        ResourceLoader.Load<Texture2D>($"res://gem-pouch/images/{gem.ToString().ToLowerInvariant()}.png");
}

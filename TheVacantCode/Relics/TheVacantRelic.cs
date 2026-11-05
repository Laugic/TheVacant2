using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheVacant.TheVacantCode.Character;
using TheVacant.TheVacantCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace TheVacant.TheVacantCode.Relics;

[Pool(typeof(TheVacantRelicPool))]
public abstract class TheVacantRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}
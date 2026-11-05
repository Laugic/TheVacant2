using BaseLib.Abstracts;
using BaseLib.Extensions;
using TheVacant.TheVacantCode.Extensions;
using Godot;

namespace TheVacant.TheVacantCode.Powers;

public abstract class TheVacantPower : CustomPowerModel
{
    //Loads from TheVacant/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
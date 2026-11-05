using BaseLib.Abstracts;
using TheVacant.TheVacantCode.Extensions;
using Godot;

namespace TheVacant.TheVacantCode.Character;

public class TheVacantRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheVacant.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
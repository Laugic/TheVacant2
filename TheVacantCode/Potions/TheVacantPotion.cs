using BaseLib.Abstracts;
using BaseLib.Utils;
using TheVacant.TheVacantCode.Character;

namespace TheVacant.TheVacantCode.Potions;

[Pool(typeof(TheVacantPotionPool))]
public abstract class TheVacantPotion : CustomPotionModel;
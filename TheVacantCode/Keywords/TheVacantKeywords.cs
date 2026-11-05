using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheVacant.TheVacantCode.Keywords;


public class TheVacantKeywords
{
    [CustomEnum, KeywordProperties((AutoKeywordPosition.Before))]
    public static CardKeyword Ricochet;

    [CustomEnum] public static StaticHoverTip Return;
}
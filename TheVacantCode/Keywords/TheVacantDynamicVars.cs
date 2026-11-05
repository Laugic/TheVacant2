using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheVacant.TheVacantCode.Cards;

namespace TheVacant.TheVacantCode.Keywords;

public class MillVar : DynamicVar
{
    public const string Key = "Mill";
    public MillVar(decimal persistCount) : base(Key, persistCount)
    {
        this.WithTooltip("THEVACANT-MILL");
    }

    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        if (!runGlobalHooks)
            return;
        PreviewValue = TheVacantCard.GetBonusFromSpirit(card.Owner, (int)BaseValue);
    }
}
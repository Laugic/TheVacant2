using MegaCrit.Sts2.Core.Models;

namespace TheVacant.TheVacantCode.Powers;

public interface OnMillPower
{
    public virtual List<CardModel> UnconditionalRicochets(List<CardModel> cardsToMill, List<CardModel> extraRicochets) => extraRicochets;
    public virtual List<CardModel> GetExtraRicochets(List<CardModel> cards, List<CardModel> extraRicochets) => extraRicochets;

    public virtual void WhenMill(){}
    
    public virtual void OnMill(CardModel card) {}
    public void OnRicochet(CardModel card) {}
}
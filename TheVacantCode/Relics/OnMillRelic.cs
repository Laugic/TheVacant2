using MegaCrit.Sts2.Core.Models;

namespace TheVacant.TheVacantCode.Relics;

public interface OnMillRelic
{
    public List<CardModel> UnconditionalRicochets(List<CardModel> cardsToMill, List<CardModel> extraRicochets) => extraRicochets;
    public List<CardModel> GetExtraRicochets(List<CardModel> cards, List<CardModel> extraRicochets) => extraRicochets;

    public void OnMill(CardModel card) {}
    
    public void OnRicochet(CardModel card) {}
}
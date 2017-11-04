using System;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

public class CardsPocket : OverrodeOperator, IDisposable
{
    public event Action OnPocketEmpty;
    private void OnPocketEmptyHandler()
    {
        if(OnPocketEmpty != null) OnPocketEmpty();
    }

    private List<Card> _pocketCards;
    private Card _trumpCard;

    public Card TrumpCard
    {
        get
        {
            return _trumpCard;
        }
    }

    public int CurrentPocketSize
    {
        get
        {
            return _pocketCards == null ? 0 : _pocketCards.Count;
        }
    }

    public CardsPocket()
    {
        Initialize();
    }

    public void Initialize()
    {
        GeneratePocket();
        if(!_pocketCards.IsNullOrEmpty()) _trumpCard = _pocketCards.GetRandomItem();
        Debug.LogFormat("<size=18><color=magenta>{0}</color></size>", _trumpCard);
    }

    public Card GetNextCard()
    {
        if(_pocketCards.IsNullOrEmpty())
        {
            OnPocketEmptyHandler();
            return null;
        }

        var temp = _pocketCards.RemoveRandomItemElse(_trumpCard);
        return temp;
    }

    public void ReturnCards(List<Card> cards)
    {
        if(cards.IsNullOrEmpty()) return;
        foreach (var item in cards)
        {
            AddCard(item);
        }
    }

    private void AddCard(Card card)
    {
        if(!card || IsContainsCard(card.CardQuality, card.CardSuit)) return;
        _pocketCards.Add(card);
    }

    public override string ToString()
    {
        return string.Format("[CardsPocket] {0}", _pocketCards.Count);
    }

    private void GeneratePocket()
    {
        if(_pocketCards.IsNull()) _pocketCards = new List<Card>();
            
        var arrayCardQuality = Enum.GetValues(typeof(CardQuality));
        var arrayCardSuit = Enum.GetValues(typeof(CardSuit));

        int counter = 0;
        foreach (var quality in arrayCardQuality)
        {
            foreach (var suit in arrayCardSuit)
            {
                var qua = (CardQuality)quality;
                var sui = (CardSuit)suit;
                if(IsContainsCard(qua,sui)) continue;
                var temp = new Card(qua, sui);
                _pocketCards.Add(temp);
                counter++;
            }
        }
        Debug.LogFormat("<size=18><color=olive>Added {0} cards.</color></size>", counter);
    }

    private bool IsContainsCard(CardQuality cardQuality, CardSuit cardSuit)
    {
        if(_pocketCards.IsNullOrEmpty()) return false;

        foreach (var item in _pocketCards)
        {
            if(!item) continue;
            if(item.IsSame(cardQuality, cardSuit)) return true;
        }
        return false;
    }

    #region IDisposable implementation

    public void Dispose()
    {
        Debug.LogFormat("<size=18><color=teal>{0}</color></size>", "Dispose cards pocket");
    }

    #endregion
}


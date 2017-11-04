using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

public class Player : OverrodeOperator, IDisposable
{
    private List<Card> _cardsPocket;
    private StringBuilder result;

    public List<Card> Cards
    {
        get
        {
            return _cardsPocket;
        }
    }

    public Player()
    {
        _cardsPocket = new List<Card>();
    }

    public Card NewMove()
    {
        if(_cardsPocket.IsNullOrEmpty()) return null;

        var temp = _cardsPocket[0];
        _cardsPocket.RemoveAt(0);
        return temp;
    }

    public void AddCard(Card card)
    {
        if(card == null) return;
        _cardsPocket.Add(card);
    }

    public CardSuit TrumpSuit
    {
        get;
        set;
    }

    public override string ToString()
    {
        if(_cardsPocket.IsNullOrEmpty()) return string.Empty;
        if(result == null) result = new StringBuilder();
        result.Remove(0, result.Length);

        foreach (var item in _cardsPocket)
        {
            if(!item) continue;
            result.Append(item.ToString());
        }

        return result.ToString();
    }

    public void SortPocket()
    {
        if(_cardsPocket.IsNullOrEmpty()) return;
        _cardsPocket.Sort(CompareCards);
    }

    private int CompareCards(Card a, Card b)
    {
        if(!a || !b) return 0;

        if(a.CardSuit == TrumpSuit && b.CardSuit != TrumpSuit) return 1;
        if(a.CardSuit != TrumpSuit && b.CardSuit == TrumpSuit) return -1;
            
        return ((int)a.CardQuality).CompareTo((int)b.CardQuality);
    }

    #region IDisposable implementation

    public void Dispose()
    {
        _cardsPocket.Clear();
        Debug.LogFormat("<size=18><color=red>Dispose player {0}</color></size>", GetHashCode());
    }

    #endregion
}


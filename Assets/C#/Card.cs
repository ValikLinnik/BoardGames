using System;
using System.Collections.Generic;
using System.Collections;

public class Card : OverrodeOperator
{
    public CardQuality CardQuality
    {
        get;
        private set;
    }

    public CardSuit CardSuit
    {
        get;
        private set;
    }

    public Card(CardQuality cardQuality, CardSuit cardSuit)
    {
        CardQuality = cardQuality;
        CardSuit = cardSuit;
    }

    public bool IsSame(CardQuality cardQuality, CardSuit cardSuit)
    {
        return CardQuality == cardQuality && CardSuit == cardSuit;
    }

    public override string ToString()
    {
        return string.Format("[{0}, {1}]", CardQuality, CardSuit);
    }
}


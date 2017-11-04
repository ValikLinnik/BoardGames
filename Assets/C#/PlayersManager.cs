using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

public class PlayersManager : OverrodeOperator, IDisposable
{
    private List<Player> _players;
    private Player currentMovePlayer;
    private CardsPocket _cardsPocket;

    public PlayersManager(int playersQuantity, CardsPocket cardsPocket)
    {
        Initialize(playersQuantity, cardsPocket);
    }

    public void Initialize(int playersQuantity, CardsPocket cardsPocket)
    {
        if(cardsPocket == null) throw new NullReferenceException("Card pocket is null");

        _cardsPocket = cardsPocket;
        _players = _players ?? new List<Player>();
        bool needToAddPlayer = _players.Count != playersQuantity;

        if(needToAddPlayer) CreatePlayers(playersQuantity, _players);

        SetStartCards(_players);
        if(!currentMovePlayer) currentMovePlayer = _players.GetRandomItem();
        Debug.LogFormat("<size=18><color=purple>{0}</color></size>", currentMovePlayer);
    }

    private void CreatePlayers(int playersQuantity, List<Player> players)
    {
        Debug.LogFormat("<size=18><color=maroon>CreatePlayers:{0}</color></size>", playersQuantity);
        players.Clear();

        for (int i = 0; i < playersQuantity; i++)
        {
            var temp = new Player();
            if(!temp) continue;
            if(_cardsPocket.TrumpCard != null) temp.TrumpSuit = _cardsPocket.TrumpCard.CardSuit;
            else throw new NullReferenceException("Trump card is null.");
            _players.Add(temp);
        }
    }

    private void SetStartCards(List<Player> players, int cardsQuantity = 6)
    {
        if(players.IsNullOrEmpty() || cardsQuantity <= 0 || _cardsPocket == null) return;

        for (int i = 0; i < cardsQuantity; i++)
        {
            foreach (var player in players)
            {
                if(!player) continue;
                var temp = _cardsPocket.GetNextCard();
                if (!temp) continue;
                player.AddCard(temp);
            }
        }

        foreach (var player in players)
        {
            if (!player) continue;
            player.SortPocket(); 
            Debug.LogFormat("<size=18><color=olive>{0}</color></size>", player);
        }
    }

    #region IDisposable implementation

    public void Dispose()
    {   
        currentMovePlayer = null;

        foreach (var player in _players)
        {
            if(!player) continue;
            _cardsPocket.ReturnCards(player.Cards);
            player.Dispose();
        }

        _cardsPocket = null;
        Debug.LogFormat("<size=18><color=silver>{0}</color></size>", "Dispose players manager");
    }

    #endregion
}


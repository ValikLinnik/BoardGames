using UnityEngine;
using System;

public class GameLogic : MonoBehaviour 
{
    [SerializeField]
    private int _playerQuantity = 2;

    #region PRIVATE FIELDS

    private CardsPocket _cardsPocket;
    private PlayersManager _playersManager;
    private bool _isGameOn;

    #endregion

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,30), _isGameOn ? "ReStart" : "Start"))
        {
            if(_isGameOn) EndGame();
            StartGame();
        }
    }

    private void StartGame()
    {
        Debug.LogFormat("<size=18><color=white>{0}</color></size>", "START GAME");
        _isGameOn = true;
        PrepareCardsPocket();
        PreparePlayers();
    }

    private void EndGame()
    {
        _isGameOn = false;
        if(_cardsPocket) _cardsPocket.Dispose();
        if(_playersManager) _playersManager.Dispose();
    }

    private void PreparePlayers()
    {
        if(!_playersManager) _playersManager = new PlayersManager(_playerQuantity, _cardsPocket);
        else _playersManager.Initialize(_playerQuantity, _cardsPocket);
    }

    private void PrepareCardsPocket()
    {
        if(_cardsPocket == null) 
        {
            _cardsPocket = new CardsPocket();
            _cardsPocket.OnPocketEmpty += OnPocketEmpty; 
            Debug.LogFormat("<size=18><color=yellow>{0}</color></size>", "new pocket and subscribe");
        }
        else _cardsPocket.Initialize();
    }
        
    private void OnPocketEmpty ()
    {
        Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "CardsPocket is empty");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    public enum GameState
    {
        INTRODUCTION,
        DETERMINE_FIRST_PLAYER,
        FINISHED_DETERMINING_FIRST_PLAYER,
        DEALING,
        MOVE_CARDS_TO_HAND,
        RUNNING,
        CURRENT_PLAYERS_TURN_NO_MARBLES_IN_PLAY,
        CURRENT_PLAYERS_TURN_MARBLES_IN_PLAY

    };
    public enum PlayerColour { Red, Green, Yellow, Blue };

    public GameState gameState;
    public CardManager cardManager;
    public MenuManager menuManger;
    private int currentPlayer;
    public int player;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.INTRODUCTION;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopUpDismissed()
    {
        switch (gameState)
        {
            case GameState.INTRODUCTION:
                gameState = GameState.DETERMINE_FIRST_PLAYER;
                cardManager.DetermineWhoGoesFirst();
                break;
            case GameState.FINISHED_DETERMINING_FIRST_PLAYER:
                cardManager.ShuffleDeck();
                cardManager.StackDeck();
                cardManager.Deal(5);
                gameState = GameState.DEALING;
                break;
            case GameState.DEALING:

                break;
            case GameState.RUNNING:
   
                break;
        }

    }

    public void DeterminedFirstPlayer(int player)
    {
        currentPlayer = player;
        CreatePopWithMessage(GetNameForPlayer(player) + " player goes first!");
        
    }

    public void CreatePopWithMessage(string message)
    {
        menuManger.ShowPopUpWithMessage(message);
    }

    private string GetNameForPlayer(int player)
    {
        if (player == 0) return "Red";
        if (player == 1) return "Green";
        if (player == 2) return "Yellow";
        if (player == 3) return "Blue";

        return "NULL";

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    public enum GameState { INTRODUCTION, DETERMINE_FIRST_PLAYER, RUNNING };

    public GameState gameState;
    public CardManager cardManager;
    public MenuManager menuManger;
    private int currentPlayer;

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
            case GameState.DETERMINE_FIRST_PLAYER:

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


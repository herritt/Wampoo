using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { INTRODUCTION, DETERMINE_FIRST_PLAYER, RUNNING };

    public GameState gameState;
    public GameObject cardManagerObject;
    private CardManager cardManager;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.INTRODUCTION;
        cardManager = cardManagerObject.GetComponent<CardManager>();
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
}

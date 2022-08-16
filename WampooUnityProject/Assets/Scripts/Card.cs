using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit { HEARTS, DIAMONDS, SPADES, CLUBS, JOKER };
public enum CardType { ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN,
JACK, QUEEN, KING, RED, BLACK };

public class Card : MonoBehaviour
{
    public int id;

    private CardManager cardManager;
    private Color startColor;
    private Color highlightColor;
    private Material mainMaterial;

    public Suit suit;
    public CardType cardType;

    public void Start()
    {
        cardManager = GameManager.Instance.cardManager;
        highlightColor = new Color(1f, 1f, .3f, .5f);
        startColor = GetComponent<MeshRenderer>().materials[0].color;

    }

    public void Init(int id, Texture texture)
    {
        this.id = id;
        mainMaterial = GetComponent<MeshRenderer>().materials[1];
        mainMaterial.mainTexture = texture;
    }

    public void OnMouseEnter()
    {
        List<GameObject> playersHand = new List<GameObject>(cardManager.playersHand);

        if (playersHand.Contains(gameObject))
        {
            //show highlight of cards that can be played


            switch (GameManager.Instance.gameState)
            {
                case GameManager.GameState.CURRENT_PLAYERS_TURN_NO_MARBLES_IN_PLAY:

                    if (cardType == CardType.ACE ||
                        cardType == CardType.KING ||
                        suit == Suit.JOKER)
                    {
                        GetComponent<MeshRenderer>().materials[0].color = highlightColor;

                    }


                    break;

            }

            
        }
    }

    public void OnMouseExit()
    {
        List<GameObject> playersHand = new List<GameObject>(cardManager.playersHand);

        if (playersHand.Contains(gameObject))
        {

            switch (GameManager.Instance.gameState)
            {
                case GameManager.GameState.CURRENT_PLAYERS_TURN_NO_MARBLES_IN_PLAY:

                    GetComponent<MeshRenderer>().materials[0].color = startColor;

                    break;

            }



        }
    }

    public void OnMouseDown()
    {
        cardManager.handlePlayerClickedCard(this);
    }

    public override string ToString()
    {
        if (suit.Equals(Suit.JOKER))
        {
            return cardType + " " + suit;
        }
        return cardType + " of " + suit;
    }

}

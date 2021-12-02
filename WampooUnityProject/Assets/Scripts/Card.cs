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
            //show highlight
            startColor = GetComponent<MeshRenderer>().materials[0].color;
            GetComponent<MeshRenderer>().materials[0].color = highlightColor;
        }
    }

    public void OnMouseExit()
    {
        List<GameObject> playersHand = new List<GameObject>(cardManager.playersHand);

        if (playersHand.Contains(gameObject))
        {
            GetComponent<MeshRenderer>().materials[0].color = startColor;
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

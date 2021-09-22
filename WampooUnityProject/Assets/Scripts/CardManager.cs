using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Texture[] textures;
    public GameObject cardPrefab;
    public GameObject[] deck;
    public GameObject[] dealPositions;
    private const int DECK_SIZE = 54;
    private const float CARD_THICKNESS = 0.05f;
    public float dealSpeed = 3;
    private Vector3 velocity = Vector3.zero;
    private float EPISOLON = 1f;

    private GameObject currentCardBeingDelt;
    private GameObject currentTarget;
    private bool determingingWhoToGoFirst = false;
    private bool dealing = false;
    private bool cardFlipped = false;
    private int currentPlayerBeingDelt;
    private int dealCounter;
    private int cardsRemainingToBeDelt;
    private Quaternion rotateTo;
    private float halfway;
    private GameObject[] playersHand;
    private int playerHandCounter = 0;
    private Hashtable suitMap;

    private enum Suit { Hearts, Diamonds, Spades, Clubs, Joker };

    // Start is called before the first frame update
    void Start()
    {
        deck = new GameObject[DECK_SIZE];
        suitMap = new Hashtable();
        playersHand = new GameObject[5];

        for (int i = 0; i < DECK_SIZE; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Card card = cardObject.GetComponent<Card>();
            Texture texture = textures[i];
            string name = texture.name;

            suitMap.Add(i, SuitFromName(name));
                 
            card.Init(i, texture);
            cardObject.transform.Rotate(90, 0, 0);
            deck[i] = cardObject;
        }

        ShuffleDeck();
        StackDeck();
    }

    private Suit SuitFromName(string name)
    {
        if (name.Contains("hearts")) return Suit.Hearts;
        if (name.Contains("diamonds")) return Suit.Diamonds;
        if (name.Contains("clubs")) return Suit.Clubs;
        if (name.Contains("joker")) return Suit.Joker;

        return Suit.Spades;       
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, DECK_SIZE);
            GameObject cardObject = deck[randomIndex];
            deck[randomIndex] = deck[i];
            deck[i] = cardObject;
        }
    }

    public void StackDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            float jitter = UnityEngine.Random.Range(0, 1f);

            GameObject cardObject = deck[i];
            cardObject.transform.position = Vector3.zero;
            cardObject.transform.rotation = Quaternion.identity;
            cardObject.transform.Rotate(90, 0, 0);

            

            cardObject.transform.Translate(new Vector3(jitter, jitter, -CARD_THICKNESS - (i * CARD_THICKNESS)));

            jitter = UnityEngine.Random.Range(-2, 2f);
            cardObject.transform.Rotate(0, 0, jitter);

            if (GameManager.Instance.player == 1 || GameManager.Instance.player == 3)
            {
                cardObject.transform.Rotate(0, 0, 90);
            }
        }

        playersHand = new GameObject[5];
    }

    public void DealNextCard(bool flipped)
    {
        currentCardBeingDelt = deck[dealCounter--];
        currentPlayerBeingDelt = (currentPlayerBeingDelt + 1) % 4;
        currentTarget = dealPositions[currentPlayerBeingDelt];

        halfway = Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position) / 2f;

        if (flipped)
        {

            cardFlipped = flipped;
            Vector3 rotationVector = new Vector3(180, 0, 0);
            rotateTo = currentCardBeingDelt.transform.rotation * Quaternion.Euler(rotationVector);

        }

        cardsRemainingToBeDelt = cardsRemainingToBeDelt - 1;

    }

    public void DetermineWhoGoesFirst()
    {
        StackDeck();
        dealCounter = DECK_SIZE - 1;
        cardsRemainingToBeDelt = DECK_SIZE;
        currentPlayerBeingDelt = UnityEngine.Random.Range(1, 5);
        determingingWhoToGoFirst = true;
        DealNextCard(true);

    }

    private bool CheckIfSpade()
    {
        Card card = currentCardBeingDelt.GetComponent<Card>();
        int id = card.id;

        Suit suit = (Suit)suitMap[id];
        if (suit.Equals(Suit.Spades)) return true;

        return false;

    }

    public void Deal(int cardsInHand)
    {
        currentPlayerBeingDelt = GameManager.Instance.player;
        cardFlipped = false;
        dealing = true;
        dealCounter = DECK_SIZE - 1;
        cardsRemainingToBeDelt = cardsInHand * 4 + 1;
        DealNextCard(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (cardsRemainingToBeDelt > 0)
        {
            int offset = (DECK_SIZE - dealCounter);
            float heightOfCardAtTarget = (CARD_THICKNESS + (offset * CARD_THICKNESS));
            currentTarget.transform.position = new Vector3(currentTarget.transform.position.x, heightOfCardAtTarget, currentTarget.transform.position.z);

            currentCardBeingDelt.transform.position = Vector3.Lerp(currentCardBeingDelt.transform.position,
                currentTarget.transform.position, Time.deltaTime * dealSpeed);

            float distance = Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position);

            if (cardFlipped && distance < halfway)
            {
                currentCardBeingDelt.transform.rotation = Quaternion.Lerp(currentCardBeingDelt.transform.rotation, rotateTo, Time.deltaTime * dealSpeed * 4);
            }
            else
            {
                currentCardBeingDelt.transform.position = Vector3.Lerp(
                    currentCardBeingDelt.transform.position,
                    currentTarget.transform.position,
                    Time.deltaTime * dealSpeed);
            }

            if (Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position) < EPISOLON)
            {
                if (determingingWhoToGoFirst)
                {
                    if (CheckIfSpade())
                    {
                        GameManager gameManager = GameManager.Instance;
                        gameManager.DeterminedFirstPlayer(currentPlayerBeingDelt);
                        determingingWhoToGoFirst = false;
                    }
                    else
                    {
                        DealNextCard(true);
                    }

                }
                
                if (dealing)
                {
                    if (currentPlayerBeingDelt  == GameManager.Instance.player)
                    {
                        //add to player's hand                    
                        playersHand[playerHandCounter++] = currentCardBeingDelt;

                        currentCardBeingDelt.transform.rotation = Quaternion.identity;
                        float handOffset = 30f;
                        if (currentPlayerBeingDelt == 0)
                        {
                            currentCardBeingDelt.transform.Rotate(-20, 180, 180);
                            currentCardBeingDelt.transform.Translate(new Vector3(-handOffset + playerHandCounter * 10, -5, 0));

                        }
                        else if(currentPlayerBeingDelt == 1)
                        {
                            currentCardBeingDelt.transform.Rotate(-20, -90, 0);
                            currentCardBeingDelt.transform.Translate(new Vector3(handOffset - playerHandCounter * 10, 5, 0));

                        }
                        else if (currentPlayerBeingDelt == 2)
                        {
                            currentCardBeingDelt.transform.Rotate(-20, 0, 180);
                            currentCardBeingDelt.transform.Translate(new Vector3(-handOffset + playerHandCounter * 10, -5, 0));

                        }
                        else
                        {
                            currentCardBeingDelt.transform.Rotate(-20, 90, 180);
                            currentCardBeingDelt.transform.Translate(new Vector3(-handOffset + playerHandCounter * 10, -5, 0));

                        }

                    }
                    DealNextCard(false);
                }
            }

        }

    }

}

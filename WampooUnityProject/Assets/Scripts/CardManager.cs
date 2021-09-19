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
    public float dealSpeed = 4F;
    private Vector3 velocity = Vector3.zero;
    private float EPISOLON = 1f;

    private GameObject currentCardBeingDelt;
    private GameObject currentTarget;
    private bool determingingWhoToGoFirst = false;
    private bool cardFlipped = false;
    private int currentPlayerBeingDelt;
    private int dealCounter;
    private Quaternion rotateTo;
    private float halfway;

    private Hashtable suitMap;

    private enum Suit { Hearts, Diamonds, Spades, Clubs, Joker };

    // Start is called before the first frame update
    void Start()
    {
        deck = new GameObject[DECK_SIZE];
        suitMap = new Hashtable();

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

    void ShuffleDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, DECK_SIZE);
            GameObject cardObject = deck[randomIndex];
            deck[randomIndex] = deck[i];
            deck[i] = cardObject;
        }
    }

    void StackDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            float jitter = UnityEngine.Random.Range(0, 1f);

            GameObject cardObject = deck[i];
            cardObject.transform.Translate(new Vector3(jitter, jitter, -CARD_THICKNESS - (i * CARD_THICKNESS)));

            jitter = UnityEngine.Random.Range(-2, 2f);
            cardObject.transform.Rotate(0, 0, jitter);

        }
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
 
    }

    public void DetermineWhoGoesFirst()
    {
        dealCounter = DECK_SIZE - 1;
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

    private void StartGameWithFirstPlayer(int firstPlayer)
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.DeterminedFirstPlayer(firstPlayer);

    }

    // Update is called once per frame
    void Update()
    {

        if (determingingWhoToGoFirst)
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
                currentCardBeingDelt.transform.position = new Vector3(currentCardBeingDelt.transform.position.x,
                    DECK_SIZE * CARD_THICKNESS,
                    currentCardBeingDelt.transform.position.z);
            }

            if (Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position) < EPISOLON)
            {
                if (CheckIfSpade())
                {
                    StartGameWithFirstPlayer(currentPlayerBeingDelt);
                    determingingWhoToGoFirst = false;
                }
                else
                {
                    DealNextCard(true);
                }
                 
            }

        }

    }

}

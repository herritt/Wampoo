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
    private float EPISOLON = 0.1f;

    private GameObject currentCardBeingDelt;
    private GameObject currentTarget;
    private bool cardFlipped = false;
    private bool finishedShowingCards = false;
    private int currentPlayerBeingDelt;
    private int dealCounter;
    private int cardsRemainingToBeDelt;
    private Quaternion rotateTo;
    private float halfway;
    private GameObject[] playersHand;
    private int playerHandCounter = 0;
    private Hashtable suitMap;
    private int[] playerYRotations = { 180, -90, 0, 90 };

    public Transform[] cardInHandPositions;
    public Transform[] showCardPositions;

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

            switch (GameManager.Instance.player)
            {
                case 0:
                    cardObject.transform.Rotate(0, 0, 180);
                    break;
                case 1:
                    cardObject.transform.Rotate(0, 0, 90);
                    break;
                case 2:
                    cardObject.transform.Rotate(0, 0, 0);
                    break;
                case 3:
                    cardObject.transform.Rotate(0, 0, -90);
                    break;

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
        dealCounter = DECK_SIZE - 1;
        cardsRemainingToBeDelt = cardsInHand * 4 + 1;
        DealNextCard(false);

    }

    private void SendCurrentCardToTarget()
    {
        int offset = (DECK_SIZE - dealCounter);
        float heightOfCardAtTarget = (CARD_THICKNESS + (offset * CARD_THICKNESS));
        currentTarget.transform.position = new Vector3(currentTarget.transform.position.x, heightOfCardAtTarget, currentTarget.transform.position.z);
        currentCardBeingDelt.transform.position = Vector3.Lerp(currentCardBeingDelt.transform.position,
            currentTarget.transform.position, Time.deltaTime * dealSpeed);

    }

    private void FlipCardIfNeeded()
    {
        float distance = Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position);

        if (cardFlipped && distance < halfway)
        {
            currentCardBeingDelt.transform.rotation = Quaternion.Lerp(currentCardBeingDelt.transform.rotation, rotateTo, Time.deltaTime * dealSpeed * 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.INTRODUCTION:
                break;
            case GameManager.GameState.DETERMINE_FIRST_PLAYER:

                if (cardsRemainingToBeDelt > 0)
                {
                    SendCurrentCardToTarget();
                    FlipCardIfNeeded();

                    if (Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position) < EPISOLON)
                    {
                        if (CheckIfSpade())
                        {
                            GameManager gameManager = GameManager.Instance;
                            gameManager.DeterminedFirstPlayer(currentPlayerBeingDelt);
                            gameManager.gameState = GameManager.GameState.FINISHED_DETERMINING_FIRST_PLAYER;
                        }
                        else
                        {
                            DealNextCard(true);
                        }
                    }

                }
 
                break;
            case GameManager.GameState.DEALING:

                if (cardsRemainingToBeDelt > 0)
                {
                    SendCurrentCardToTarget();

                    if (currentPlayerBeingDelt == GameManager.Instance.player)
                    {
                        //deal card
                        StartCoroutine(DealPlayerCard(currentCardBeingDelt, playerHandCounter, result => 
                        {
                            //TODO: fix magic number
                            if (result == 4)
                            {
                                GameManager.Instance.gameState = GameManager.GameState.MOVE_CARDS_TO_HAND;
                            }
                        }));

                        //add to player's hand                    
                        playersHand[playerHandCounter++] = currentCardBeingDelt;

                        //deal next card
                        DealNextCard(false);
                    }

                    if (Vector3.Distance(currentCardBeingDelt.transform.position, currentTarget.transform.position) < EPISOLON)
                    {
                        DealNextCard(false);
                    }
                }


                break;
            case GameManager.GameState.MOVE_CARDS_TO_HAND:
                float cardMoveSpeed = 1f;

                for (int i = 0; i < cardInHandPositions.Length; i++)
                {
                    if (cardInHandPositions[i] != null)
                    {
                        StartCoroutine(
                        Animate(playersHand[i], cardInHandPositions[i].position, cardMoveSpeed,
                        null

                        ));

                        StartCoroutine(
                            Rotate(playersHand[i], cardInHandPositions[i].rotation, cardMoveSpeed, null));
                    }
                }
                break;
            case GameManager.GameState.RUNNING:
                break;
        }

    }

    IEnumerator DealPlayerCard(GameObject cardBeingDelt, int index, Action<int> callback)
    {
        Quaternion startRotation = currentCardBeingDelt.transform.rotation;
        currentCardBeingDelt.transform.rotation = Quaternion.identity;
        currentCardBeingDelt.transform.Rotate(-20, playerYRotations[currentPlayerBeingDelt], 0);
        Quaternion stopRotation = currentCardBeingDelt.transform.rotation;
        currentCardBeingDelt.transform.rotation = startRotation;

        Vector3 finalPosition = showCardPositions[index].position;


        GameObject cardRef = currentCardBeingDelt;
        yield return StartCoroutine(
            Animate(currentCardBeingDelt, finalPosition, dealSpeed,
            result =>
            {
                StartCoroutine(Rotate(cardRef, stopRotation, dealSpeed, rotateResult => 
                {
                    callback(index);

                }));
                
            }
            ));   

    }

    IEnumerator Rotate(GameObject obj, Quaternion stopRotation, float speed, Action<int> callback)
    {

        while (Quaternion.Angle(obj.transform.rotation, stopRotation) >= EPISOLON)
        {
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, stopRotation, Time.deltaTime * speed);
            yield return null;
        }

        callback(1);

    }


    IEnumerator Animate(GameObject obj, Vector3 stopPosition, float speed, Action<int> callback)
    {
        while (Vector3.Distance(obj.transform.position, stopPosition) >= EPISOLON)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, stopPosition, Time.deltaTime * speed);

            yield return null;
        }

        callback(1);

    }
}



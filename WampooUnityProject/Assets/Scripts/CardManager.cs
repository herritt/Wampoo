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
    private bool sendingCardToPlayer = false;
    private bool determingingWhoToGoFirst = false;
    private bool cardFlipped = false;
    private int currentPlayerBeingDelt;
    private int dealCounter;
    private Quaternion rotateTo;
    private float halfway;


    // Start is called before the first frame update
    void Start()
    {
        deck = new GameObject[DECK_SIZE];

        for (int i = 0; i < DECK_SIZE; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Card card = cardObject.GetComponent<Card>();
            card.Init(i, textures[i]);
            cardObject.transform.Rotate(90, 0, 0);
            deck[i] = cardObject;
        }

        ShuffleDeck();
        StackDeck();

        foreach (GameObject cardObject in deck)
        {
            Card card = cardObject.GetComponent<Card>();
            Debug.Log(card.id);
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            int randomIndex = Random.Range(0, DECK_SIZE);
            GameObject cardObject = deck[randomIndex];
            deck[randomIndex] = deck[i];
            deck[i] = cardObject;
        }
    }

    void StackDeck()
    {
        for (int i = 0; i < DECK_SIZE; i++)
        {
            float jitter = Random.Range(0, 1f);

            GameObject cardObject = deck[i];
            cardObject.transform.Translate(new Vector3(jitter, jitter, -CARD_THICKNESS - (i * CARD_THICKNESS)));

            jitter = Random.Range(-2, 2f);
            cardObject.transform.Rotate(0, 0, jitter);

        }
    }

    public void DealNextCard(bool flipped)
    {
        currentCardBeingDelt = deck[dealCounter--];
        currentPlayerBeingDelt = (currentPlayerBeingDelt + 1) % 4;
        currentTarget = dealPositions[currentPlayerBeingDelt];
        sendingCardToPlayer = true;

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
        currentPlayerBeingDelt = Random.Range(1, 5);
        determingingWhoToGoFirst = true;
        sendingCardToPlayer = true;
        DealNextCard(true);

        //if its not a spade, go to next person

        //if it is a spade, this person goes first - let game manager know
    }

    private void CheckIfSpade()
    {
        DealNextCard(true);

    }
    // Update is called once per frame
    void Update()
    {

        if (determingingWhoToGoFirst)
        {
            if (sendingCardToPlayer)
            {
                int offset = (DECK_SIZE - dealCounter);
                float heightOfCardAtTarget = (CARD_THICKNESS + (offset * CARD_THICKNESS));
                currentTarget.transform.position = new Vector3(currentTarget.transform.position.x, heightOfCardAtTarget, currentTarget.transform.position.z );

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
                    sendingCardToPlayer = false;
                    cardFlipped = false;
                }

            }
            else
            {
                CheckIfSpade();
            }

        }

    }
}

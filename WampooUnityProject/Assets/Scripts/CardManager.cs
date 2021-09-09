using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Texture[] textures;
    public GameObject cardPrefab;
    public GameObject[] deck;
    private const int DECK_SIZE = 54;
    private const float CARD_THICKNESS = 0.05f;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

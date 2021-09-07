using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Texture[] textures;
    public GameObject cardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 54; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Card card = cardObject.GetComponent<Card>();
            card.Init(i, textures[i]);
            cardObject.transform.Translate(new Vector3(20 * i, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int id;
    public GameObject cardManager;

    public void Start()
    {
        //update the card texture based on id
        Texture[] textures = cardManager.GetComponent<CardTextures>().textures;
        GetComponent<MeshRenderer>().materials[1].mainTexture = textures[id];
    }

}

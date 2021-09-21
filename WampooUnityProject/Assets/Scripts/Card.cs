using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int id;
    public GameObject cardManager;

    public void Start()
    {

    }

    public void Init(int id, Texture texture)
    {
        this.id = id;
        GetComponent<MeshRenderer>().materials[1].mainTexture = texture;
    }

}

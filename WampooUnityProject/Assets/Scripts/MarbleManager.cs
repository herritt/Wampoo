using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour
{
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = material;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Random.Range(0f,1f), Random.Range(0f, 1f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


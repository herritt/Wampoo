using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInAndOut : MonoBehaviour
{
    public float MAX_SCALE;
    public float MIN_SCALE;
    public float scaleChangeAmount;
    private Vector3 scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        scaleChangeAmount /= 10000f;
        scaleChange = new Vector3(scaleChangeAmount, scaleChangeAmount, scaleChangeAmount);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += scaleChange;

        if (transform.localScale.y < MIN_SCALE || transform.localScale.y > MAX_SCALE)
        {
            scaleChange = -scaleChange;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInAndOut : MonoBehaviour
{
    public float MAX_SCALE = 1.0f;
    public float MIN_SCALE = 0.9f;
    public float scaleChangeAmount = 0.5f;
    private Vector3 scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        scaleChangeAmount = Mathf.Abs(scaleChangeAmount);
        scaleChange = new Vector3(scaleChangeAmount, scaleChangeAmount, scaleChangeAmount);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += scaleChange * Time.deltaTime;

        if (transform.localScale.y < MIN_SCALE || transform.localScale.y > MAX_SCALE)
        {
            scaleChange = -scaleChange;
        }
        
    }
}

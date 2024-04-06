using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var s = transform.localScale;
        s.x = 2.0f * (float)Math.Cos((float)Time.fixedTime);
        transform.localScale = s;
    }
}

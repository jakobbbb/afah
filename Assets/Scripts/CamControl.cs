using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x / Screen.width;
        float lim = 0.2f;  // lol this looks like math
        float dx = 0f;
        float speed_max = 10f / lim;
        if (x < lim) {
            dx = -speed_max * (lim -x );
        } else if (x > (1f-lim)) {
            dx = - speed_max * ((1f-lim) - x);
        }
        transform.Translate(new Vector3(Time.deltaTime * dx, 0, 0));
    }
}

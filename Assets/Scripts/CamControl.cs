using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    [SerializeField]
    private Transform m_MinIndoor;
    [SerializeField]
    private Transform m_MaxIndoor;
    [SerializeField]
    private Transform m_MinOutdoor;
    [SerializeField]
    private Transform m_MaxOutdoor;
    void Update() {
        bool indoor = true;
        var min = indoor ? m_MinIndoor : m_MinOutdoor;
        var max = indoor ? m_MaxIndoor : m_MaxOutdoor;

        float x = Input.mousePosition.x / Screen.width;
        float lim = 0.2f;  // lol this looks like math
        float dx = 0f;
        float speed_max = 10f / lim;
        if (x < lim) {
            dx = -speed_max * (lim -x );
        } else if (x > (1f-lim)) {
            dx = - speed_max * ((1f-lim) - x);
        }
        float new_x = transform.position.x + (Time.deltaTime * dx);
        if (new_x < min.position.x || new_x > max.position.x) {
            return;
        }
        transform.Translate(new Vector3(Time.deltaTime * dx, 0, 0));
    }
}

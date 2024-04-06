using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    private float m_Capacity = 20f;
    
    public void Consume(float amount) {
        m_Capacity -= amount;
    }
    void Update() {
        transform.localScale = 0.1f * m_Capacity * Vector3.one;
        if (Input.GetKeyDown(KeyCode.Space)) {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = transform.position.z;
            transform.position = world;
        }
        if (m_Capacity < 0f) {
            Destroy(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Food : MonoBehaviour {

    private float m_Capacity = 8f;
    [SerializeField]
    private Sprite m_EmptySprite;
    [SerializeField]
    private SpriteRenderer m_Rend;

    
    public void Consume(float amount) {
        m_Capacity -= amount;
    }
    void Update() {
        //transform.localScale = 0.1f * m_Capacity * Vector3.one;
        if (Input.GetKeyDown(KeyCode.Space)) {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = transform.position.z;
            transform.position = world;
        }
        if (Empty()) {
            m_Rend.sprite = m_EmptySprite;
        }
    }

    public bool Empty() {
        return m_Capacity < 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour {

    public enum CatState {
        WALKING_TO_TARGET,
    }

    [SerializeField]
    private Transform m_NavTarget;
    [SerializeField]
    private Transform m_NavBase;

    private float m_MaxVelocity = 4f;

    void Start() {
        Debug.Log("meow");
        
    }

    void Update() {
        if (true || Input.GetKeyDown(KeyCode.Space)) {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = m_NavTarget.position.z;
            m_NavTarget.position = world;
        }
    }

    void FixedUpdate() {
        Vector3 move = Vector3.zero;
        var delta = m_NavTarget.position - m_NavBase.position;
        var velocity = m_MaxVelocity;
        var delta_norm = delta.normalized;
        var delta_x = velocity * new Vector3(delta_norm.x, 0, 0) * Time.fixedDeltaTime;
        var delta_y = velocity * new Vector3(0, delta_norm.y, 0) * Time.fixedDeltaTime;

        if (delta.magnitude < 0.1f) {
            // Target reached
            return;
        }
        
        if (CatManager.Instance.IsWalkable(m_NavBase.position + delta_x)) {
            move += delta_x;
        } else {
            Debug.Log("uwu");
        }
        if (CatManager.Instance.IsWalkable(m_NavBase.position + delta_y)) {
            move += delta_y;
        } else {
            Debug.Log("owo");
        }
        transform.Translate(move);

        
    }
}

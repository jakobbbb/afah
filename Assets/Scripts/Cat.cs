using System;
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
        Vector2 move = Vector2.zero;
        Vector2 delta = m_NavTarget.position - m_NavBase.position;
        var velocity = m_MaxVelocity;
        var delta_norm = delta.normalized;
        var delta_x = velocity * new Vector2(delta_norm.x, 0) * Time.fixedDeltaTime;
        var delta_y = velocity * new Vector2(0, delta_norm.y) * Time.fixedDeltaTime;

        if (delta.magnitude < 0.1f) {
            // Target reached
            return;
        }
        
        if (CatManager.Instance.IsWalkable((Vector2)m_NavBase.position + delta_x)) {
            move += delta_x;
        } else {
            Debug.Log("uwu");
        }
        if (CatManager.Instance.IsWalkable((Vector2)m_NavBase.position + delta_y)) {
            move += delta_y;
        } else {
            Debug.Log("owo");
        }

        foreach (var coll in CatManager.Instance.GetWalkable()) {
            var closest = coll.ClosestPoint(m_NavBase.position);
            var jump_to_target = (Vector2)m_NavTarget.position - closest;
            var current_to_jump = closest - (Vector2)m_NavBase.position;

            if (Math.Abs(current_to_jump.x) < 0.1f && jump_to_target.magnitude < delta.magnitude) {
                move = current_to_jump;
                break;
            }
        }

        transform.Translate(move);

        if (!CatManager.Instance.IsWalkable(transform.position)) {
            float m = 1000000f;
            Vector2 c = Vector2.zero;
            foreach (var coll in CatManager.Instance.GetWalkable()) {
                var closest = (Vector2)coll.ClosestPoint(m_NavBase.position);
                var dist = (closest - (Vector2)m_NavBase.position).magnitude;
                if (m > dist) {
                    m = dist;
                    c = closest;
                }
            }
            if (m < 100000f) {
                transform.position = c;
            }
        }
        
    }
}

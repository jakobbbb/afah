using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour {
    private Collider2D m_CurrentCollider;
    public enum CatState {
        WALKING_TO_TARGET,
        SLEEPING,
    }

    private CatState m_State = CatState.WALKING_TO_TARGET;

    private float m_JumpCooldown;
    private float m_StateChangeRollCooldown = 0f;
    private float m_SleepTime = 0f;

    [SerializeField]
    private Transform m_NavTarget;
    [SerializeField]
    private Transform m_NavBase;

    private float m_MaxVelocity = 4f;

    void Start() {
        Debug.Log("meow");
        m_MaxVelocity = UnityEngine.Random.Range(2f, 4f);
        CatManager.Instance.RegisterCat(this);
        m_StateChangeRollCooldown = UnityEngine.Random.Range(0f, 5f);
        m_JumpCooldown = UnityEngine.Random.Range(0f, 0.5f);
    }

    void Update() {
    }

    void FixedUpdate() {
        Unstuck();
        if (m_StateChangeRollCooldown < 0f) {
            if (RollForStateChange()) {
                ApplyStateChange();
            }
            Debug.Log("sate");
            m_StateChangeRollCooldown = 5f;
        }
        
        switch (m_State) {
            case CatState.WALKING_TO_TARGET:
                MoveTowardsGoal();
                break;
            case CatState.SLEEPING:
            break;
        }
        UpdateClosest();
        Unstuck();

        m_JumpCooldown -= Time.fixedDeltaTime;
        m_StateChangeRollCooldown -= Time.fixedDeltaTime;
        m_SleepTime += Time.fixedDeltaTime;
    }

    void ApplyStateChange() {
        var new_state = m_State;
        var scale = transform.localScale;
        
        var fy = (new_state == CatState.SLEEPING) ? 0.25f : 4.0f;
        scale.y = fy * scale.y;

        transform.localScale = scale;
    }

    bool RollForStateChange() {
        var old_state = m_State;
        if (m_State == CatState.WALKING_TO_TARGET && Util.Roll(3)) {
            m_SleepTime = 0f;
            m_State = CatState.SLEEPING;
        }
        if (m_State == CatState.SLEEPING && (Util.Roll(5) || m_SleepTime > 10.0f)) {
            m_State = CatState.WALKING_TO_TARGET;
        }

        bool changed = old_state != m_State;
        return changed;
    }

    void UpdateClosest() {
        float closest_dist = 999999f;
        Collider2D closest = null;
        foreach (var c in CatManager.Instance.GetWalkable()) {
            c.GetComponent<SpriteRenderer>().color = Color.white;
            Vector3 p = c.bounds.ClosestPoint(m_NavBase.position);
            p.z = 0f;
            var dist = (p - m_NavBase.position).magnitude;
            if (dist < closest_dist) {
                closest = c;
                closest_dist = dist;
            }
        }
        if (closest != null) {
            m_CurrentCollider = closest;
            m_CurrentCollider.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void Unstuck() {
        if (CatManager.Instance.IsWalkable(m_NavBase.position)) {
            return;
        }

        // find closest collider and move towards its center
        var dir = (m_CurrentCollider.bounds.center - m_NavBase.position).normalized;
        transform.Translate(dir * Time.fixedDeltaTime);
    }

    Vector2 AvoidOthers() {
        Vector2 move = Vector2.zero;
        foreach (Cat cat in CatManager.Instance.GetCats()) {
            var delta = transform.position - cat.transform.position;
            var dist = delta.magnitude;
            if (dist < 0.6f) {
                move += (Vector2)delta.normalized;
            }
        }
        if (move.magnitude < 0.1f) {  // hysterese let's go
            return Vector2.zero;
        }
        return 0.05f * move.normalized;
    }

    void MoveTowardsGoal() {
        Vector2 move = AvoidOthers();
        Vector2 delta = m_NavTarget.position - m_NavBase.position;
        var velocity = m_MaxVelocity;
        var delta_norm = delta.normalized;
        var delta_x = velocity * new Vector2(delta_norm.x + move.x, 0) * Time.fixedDeltaTime;
        var delta_y = velocity * new Vector2(0, delta_norm.y + move.y) * Time.fixedDeltaTime;

        var scale = transform.localScale;
        if (delta_x.x > 0f && scale.x < 0f) {
            scale.x = -scale.x;
        } else if (delta_x.x < 0f && scale.x > 0f) {
            scale.x = -scale.x;
        }
        transform.localScale = scale;

        if (delta.magnitude < 0.1f) {
            // Target reached
            return;
        }
        
        if (CatManager.Instance.IsWalkable((Vector2)m_NavBase.position + delta_x)) {
            move += delta_x;
        }
        if (CatManager.Instance.IsWalkable((Vector2)m_NavBase.position + delta_y)) {
            move += delta_y;
        }

        // jump
        if (m_JumpCooldown <= 0f) {
            Vector2 best_jump = Vector2.zero;
            float best_jump_gain = 0f;  // how closer the cat would move if it jumped
            foreach (var coll in CatManager.Instance.GetWalkable()) {
                var closest = coll.ClosestPoint(m_NavBase.position);
                var jump_to_target = (Vector2)m_NavTarget.position - closest;
                var current_to_jump = closest - (Vector2)m_NavBase.position;
                var jump_gain = delta.magnitude - jump_to_target.magnitude;
                if (jump_gain < 0f || jump_gain < best_jump_gain) {
                    continue;
                }

                if (Math.Abs(current_to_jump.x) < 0.1f && jump_to_target.magnitude < delta.magnitude) {
                    best_jump = current_to_jump;
                    best_jump_gain = jump_gain;
                }
            }
            if (best_jump_gain > 0f) {
                move = best_jump;
                m_JumpCooldown = 0.3f;  // seconds
            }
        }

        transform.Translate(move);
        
    }
}

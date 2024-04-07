using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    public bool IsAttached;

    public Collider2D Collider {
        get; private set;
    }

    void Start() {
        DragManager.Instance.Register(this);
        Collider = GetComponent<Collider2D>();
    }

    void OnDestroy() {
        DragManager.Instance.Deregister(this);
    }

    void Update() {
        if (IsAttached) {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = transform.position.z;
            transform.position = world;
            if (GetComponent<Cat>() != null) {
                GetComponent<Cat>().Collected = true;
            }
        }
    }
}

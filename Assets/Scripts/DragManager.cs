using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragManager : MonoBehaviour {
    public static DragManager Instance = null;

    private List<Draggable> m_Draggables = new();

    public Draggable CurrentlyHeld { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    public void Register(Draggable d) {
        m_Draggables.Add(d);
    }
    public void Deregister(Draggable d) {
        if (m_Draggables.Contains(d)) {
            m_Draggables.Remove(d);
        }
    }

    void Update() {
        Drag();
    }
    void Drag() {


        Draggable attached = null;
        foreach (var d in m_Draggables) {
            if (d.IsAttached) {
                attached = d;
                break;
            }
        }

        if (!Input.GetMouseButton(0)) {
            if (attached) {
                attached.IsAttached = false;
            }
            return;
        } else if (attached) {
            return;
        }

        var cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float min_z = 999999f;  // lowest y is frontmost
        Draggable under_mouse = null;
        foreach (var d in m_Draggables) {
            cursor.z = d.Collider.bounds.center.z;
            if (d.Collider.bounds.Contains(cursor) && cursor.z < min_z) {
                min_z = cursor.z;
                under_mouse = d;
            }
            d.IsAttached = false;
        }

        if (under_mouse == null) {
            return;
        }

        under_mouse.IsAttached = true;
        CurrentlyHeld = under_mouse;
    }
}
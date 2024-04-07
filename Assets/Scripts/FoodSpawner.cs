using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject m_FoodPrefab;
    [SerializeField]
    private BoxCollider2D m_ColliderSpawn;
    [SerializeField]
    private BoxCollider2D m_ColliderDelete;

    void Start() {
    }

    private void SpawnFood() {
        GameObject food = Instantiate<GameObject>(m_FoodPrefab);
        food.GetComponent<Draggable>().IsAttached = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = m_ColliderSpawn.bounds.center.z;
            if (m_ColliderSpawn.bounds.Contains(world)) {
                SpawnFood();
            }
        }
        if (Input.GetMouseButton(0)) {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = m_ColliderDelete.bounds.center.z;
            if (m_ColliderDelete.bounds.Contains(world) && DragManager.Instance.CurrentlyHeld) {
                var held_food = DragManager.Instance.CurrentlyHeld.GetComponent<Food>();
                var d = held_food.GetComponent<Draggable>();
                if (held_food && d && d.isActiveAndEnabled  && d.IsAttached) {
                    Destroy(held_food.gameObject);
                    Debug.Log("food be gone");
                }
            }
        }
    }
}

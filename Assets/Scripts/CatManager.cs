using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour {
    public static CatManager Instance = null;

    [SerializeField]
    private List<ScriptableCat> m_AvailableCats = new();
    [SerializeField]
    private Cat m_CatPrefab;

    private List<BoxCollider2D> m_WalkableColliders = new();

    public List<BoxCollider2D> GetWalkable() {
        return m_WalkableColliders;
    }

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }

        foreach (var collider in FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None)) {
            if (collider.tag.Equals("CatWalkable")) {
                m_WalkableColliders.Add(collider);
            }
        }
        Debug.Log("Found " + m_WalkableColliders.Count + " kitty-compatible colliders :3");
    }

    public bool IsWalkable(Vector3 pos) {
        foreach (var collider in m_WalkableColliders) {
            pos.z = collider.bounds.center.z;  // ignore z
            if (collider.bounds.Contains(pos)) {
                return true;
            }
        }
        return false;
    }

    
}
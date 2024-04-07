using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CatManager : MonoBehaviour {
    public static CatManager Instance = null;

    [SerializeField]
    private Cat m_CatPrefab;

    private List<BoxCollider2D> m_WalkableColliders = new();

    private List<Cat> m_Cats = new();

    [SerializeField]
    private List<GameObject> m_SpawnableCats_Level0;
    [SerializeField]
    private List<GameObject> m_SpawnableCats_Level1;
    [SerializeField]
    private List<GameObject> m_SpawnableCats_Level2;
    [SerializeField]
    private List<GameObject> m_SpawnableCats_Level3;

    public int InsanityLevel { get; private set; } 


    [SerializeField]
    private BoxCollider2D m_Outside;
    private List<BoxCollider2D> m_Outsides = new();

    public List<BoxCollider2D> GetWalkable(Cat cat) {
        if (cat.Collected) {
            return m_WalkableColliders;
        } else {
            return m_Outsides;
        }
    }

    public List<Cat> GetCats() {
        return m_Cats;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        foreach (var collider in FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None)) {
            if (collider.tag.Equals("CatWalkable")) {
                if (collider != m_Outside) {
                    m_WalkableColliders.Add(collider);
                }
            }
        }
        Debug.Log("Found " + m_WalkableColliders.Count + " kitty-compatible colliders :3");
        m_Outsides.Add(m_Outside);
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

    public void RegisterCat(Cat cat) {
        m_Cats.Add(cat);
    }

    public void SpawnCat() {
        SpawnCatSingle();
        if (m_Cats.Count >= 5) {
            // spawn two after day 6
            SpawnCatSingle();
        }
    }
    private void SpawnCatSingle() {
        List<GameObject> prefabs = null;
        switch(InsanityLevel) {
            case 0:
                prefabs = m_SpawnableCats_Level0;
                break;
            case 1:
                prefabs = m_SpawnableCats_Level1;
                break;
            case 2:
                prefabs = m_SpawnableCats_Level2;
                break;
            case 3:
                prefabs = m_SpawnableCats_Level3;
                break;
        }
        if (prefabs == null || prefabs.Count == 0) {
            GameManager.Instance.DoneCollecting();
        }
        if (prefabs.Count == 1) {
            InsanityLevel++;
        }
        int i = UnityEngine.Random.Range(0, prefabs.Count);
        GameObject prefab = prefabs[i];
        var cat = Instantiate<GameObject>(prefab, m_Outside.bounds.center, Quaternion.identity);
        var cat_name = cat.name.Split("(")[0];
        cat.name = cat_name;
        var vars = FindAnyObjectByType<InMemoryVariableStorage>();
        vars.SetValue("$" + cat.name, true);
        Debug.Log("Meow" + cat.name);
        prefabs.Remove(prefab);
        cat.GetComponent<Cat>().SetHungy(false);
    }

    
}
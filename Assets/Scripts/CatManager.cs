using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour {
    public static CatManager Instance = null;

    [SerializeField]
    private List<ScriptableCat> m_AvailableCats = new();
    [SerializeField]
    private Cat m_CatPrefab;

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    
}
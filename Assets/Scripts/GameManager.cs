using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour {

    public enum GameState {
        DIALOGUE,
        FEEDING,
        COLLECTING,
    }

    public bool IsInside { get; private set; }

    private GameState m_State = GameState.DIALOGUE;

    public static GameManager Instance;
    private DialogueRunner m_Dialogue;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
        IsInside = true;
    }

    void Start() {
        m_Dialogue = FindAnyObjectByType<DialogueRunner>();
    }

    [YarnCommand("collectcats")]
    public static void CollectCats() {
        Instance.m_State = GameState.COLLECTING;
        Instance.IsInside = false;
        CatManager.Instance.SpawnCat();

        Instance.ApplyStateChange();
    }

    public void DoneCollecting() {
        Instance.IsInside = true;
        m_State = GameState.DIALOGUE;

        Instance.ApplyStateChange();
    }

    private void ApplyStateChange() {
        m_Dialogue.GetComponentInChildren<Canvas>().enabled = m_State == GameState.DIALOGUE;
    }

    void Update() {
        if (m_State == GameState.COLLECTING) {
            foreach (var cat in CatManager.Instance.GetCats()) {
                if (!cat.Collected) {
                    return;
                }
            }
            DoneCollecting();
        }
    }
}

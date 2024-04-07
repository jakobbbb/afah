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

    [SerializeField]
    private AudioClip m_AudioOutside;
    [SerializeField]
    private AudioClip[] m_AudioInside;
    [SerializeField]
    private AudioClip[] m_SoundEffects;

    [SerializeField]
    private AudioSource m_EffectSource;
    [SerializeField]
    private AudioSource m_AmbientSource;

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

    [YarnCommand("playsound")]
    public static void PlaySound(string soundname) {
        foreach (var sound in Instance.m_SoundEffects) {
            if (sound.name == soundname) {
                Instance.m_EffectSource.PlayOneShot(sound);
                return;
            }
        }
        Debug.LogError("Sound " + soundname + " not found!!!");
    }

    [YarnCommand("collectcats")]
    public static void CollectCats() {
        Debug.Log("Collecting...");
        Instance.m_State = GameState.COLLECTING;
        Instance.IsInside = false;
        CatManager.Instance.SpawnCat();

        Instance.ApplyStateChange();
    }

    [YarnCommand("feedcats")]
    public static void FeedCats() {
        Debug.Log("Feeding...");
        Instance.IsInside = true;

        foreach (var cat in CatManager.Instance.GetCats()) {
            cat.SetHungy(true);
        }
        Instance.m_State = GameState.FEEDING;

        Instance.ApplyStateChange();
    }

    public void DoneCollecting() {
        Instance.IsInside = true;
        m_State = GameState.DIALOGUE;

        Instance.ApplyStateChange();
        Debug.Log("...done collecting and feeding.");
    }

    private void ApplyStateChange() {
        m_Dialogue.GetComponentInChildren<Canvas>().enabled = m_State == GameState.DIALOGUE;
        if (m_State != GameState.FEEDING) {
            foreach (var cat in CatManager.Instance.GetCats()) {
                cat.SetHungy(false);
            }
        }
    }

    void Update() {
        if (m_State == GameState.COLLECTING) {
            foreach (var cat in CatManager.Instance.GetCats()) {
                if (!cat.Collected) {
                    return;
                }
            }
            DoneCollecting();
        } else if (m_State == GameState.FEEDING) {
            foreach (var cat in CatManager.Instance.GetCats()) {
                if (cat.Hungy()) {
                    return;
                }
            }
            DoneCollecting();

        }
    }
}

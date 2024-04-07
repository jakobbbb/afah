using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private TMP_Text m_DayText;

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
    private List<AudioClip> m_Meows = new();

    public static GameManager Instance;
    private DialogueRunner m_Dialogue;
    [System.Serializable]
    public struct CanvasThing {
        public string canvasName;
        public Canvas canvas;
    }
    [SerializeField]
    private List<CanvasThing> m_Canvasses = new();

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
        m_AmbientSource.clip = m_AudioInside[0];
        m_AmbientSource.Play();
        foreach (var sound in Instance.m_SoundEffects) {
            if (sound.name.StartsWith("Meow")) {
                m_Meows.Add(sound);
            }
        }
    }

    private void CheckMeow() {
        if (Util.Roll(2)) {
            if (Util.Roll(CatManager.Instance.GetCats().Count)) {
                Meow();
            }
        }
    }

    public void Meow() {
        int i = UnityEngine.Random.Range(0, m_Meows.Count);
        m_EffectSource.PlayOneShot(m_Meows[i]);
        Debug.Log("meowwww");
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
        Instance.m_AmbientSource.clip = Instance.m_AudioOutside;
        Instance.m_AmbientSource.Play();
        Debug.Log("Collecting...");
        Instance.m_State = GameState.COLLECTING;
        Instance.IsInside = false;
        CatManager.Instance.SpawnCat();

        Instance.ApplyStateChange();
    }

    [YarnCommand("canvas")]
    public static void ChangeCanvasVisible(string canvas_name, bool show) {
        Debug.Log("canvassss " + canvas_name + " " + show);
        foreach (CanvasThing e in Instance.m_Canvasses) {
            Debug.Log("..." + e.canvasName + ", " + canvas_name);
            e.canvas.enabled = show && (e.canvasName == canvas_name);
            if (show && canvas_name == "day") {
                var vars = FindAnyObjectByType<InMemoryVariableStorage>();
                string day = "";
                vars.TryGetValue<string>("$day", out day);
                Instance.m_DayText.text = day;
            }
        }
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
        int i = CatManager.Instance.InsanityLevel - 1;
        Instance.m_AmbientSource.clip = Instance.m_AudioInside[i];
        Instance.m_AmbientSource.Play();
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
        CheckMeow();
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

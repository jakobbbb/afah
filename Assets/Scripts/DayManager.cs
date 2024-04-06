using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance = null;

    [SerializeField]
    private uint m_Day = 0;
    private DialogueRunner m_DiagRun;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    void Start() {
        m_DiagRun = FindFirstObjectByType<DialogueRunner>();

    }

    public void StartDay(uint day) {
        m_Day = day;
        m_DiagRun.Stop();
        m_DiagRun.StartDialogue("day" + day + "_morning");
    }

    [YarnCommand("goodnight")]
    public static void OnDayFinished() {
        Instance.StartDay(Instance.m_Day + 1);
    }
}

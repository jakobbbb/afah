using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsaneFurniture : MonoBehaviour {
    [SerializeField]
    private Sprite m_Normal;
    [SerializeField]
    private Sprite m_Insane;
    [SerializeField]
    private Sprite m_InsaneInsaneAsFuck;

    public void UpdateInsanity(int insaneeee) {
        Sprite s = m_Normal;
        if (insaneeee == 1) {
            s = m_Insane;
        } else if (insaneeee == 2) {
            s = m_InsaneInsaneAsFuck;
        }
        GetComponent<SpriteRenderer>().sprite = s;
    }
}

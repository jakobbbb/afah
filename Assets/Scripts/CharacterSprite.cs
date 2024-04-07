using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour {
    private Image m_Image;

    [SerializeField]
    private TMP_Text m_Text;

    [System.Serializable]
    public struct Character {
        public string characterName;
        public Sprite characterSprite;
    }

    public List<Character> m_Chars = new();

    void Start() {
       m_Image = GetComponent<Image>();
    }

    void Update() {
        foreach (var c in m_Chars) {
            if(m_Text && m_Text.text == c.characterName) {
                m_Image.enabled = true;
                m_Image.sprite = c.characterSprite;
                return;
            }
        }
        m_Image.enabled = false;
    }
}

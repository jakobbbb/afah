using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class CounterText : MonoBehaviour {
    private TMP_Text m_Text;
    private InMemoryVariableStorage m_Vars;
    void Start() {
       m_Text = GetComponent<TMP_Text>(); 
       m_Vars = FindAnyObjectByType<InMemoryVariableStorage>();
    }

    void Update() {
        System.Single money;
        m_Vars.TryGetValue<System.Single>("$money", out money);
        m_Text.text = money + "$";
    }
}

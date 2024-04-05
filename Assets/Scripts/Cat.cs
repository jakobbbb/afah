using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour {

    private NavMeshAgent m_Agent;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("meow");
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {
    public static bool Roll(int percent_chance) {
        return UnityEngine.Random.Range(0.0f, 1.0f) < 0.01f * (float)percent_chance;
    }
}

using UnityEngine;
using TMPro;

public class RandomFaceScript : MonoBehaviour {
    public TMP_Text textUI;

    public enum NPCFace {
        Happy,
        Sad,
        angry,
        depressed,
        laughing
    }

    NPCFace RandomFace() {
        NPCFace[] values = (NPCFace[])System.Enum.GetValues(typeof(NPCFace));
        return values[Random.Range(0, values.Length)];
    }

    void Start() {
        textUI.text = RandomFace().ToString();
    }
}
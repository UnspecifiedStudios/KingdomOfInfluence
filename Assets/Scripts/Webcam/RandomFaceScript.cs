using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RandomFaceScript : MonoBehaviour {
    public TMP_Text textUI;
    public Button button;

    public enum NPCFace {
        Happy,
        Sad,
        surprised
    }

    void Start() {
        button.onClick.AddListener(() => {
            StartCoroutine(EmotionsList());
        });
    }

    IEnumerator EmotionsList() {
        foreach (NPCFace face in System.Enum.GetValues(typeof(NPCFace))) {
            textUI.text = face.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}
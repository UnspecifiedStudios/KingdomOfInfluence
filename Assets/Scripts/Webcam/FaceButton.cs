using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static RandomFaceScript;

public class FaceButton : MonoBehaviour {
    public List<Button> buttonList;
    public List<TMP_Text> buttonTextList;
    public RandomFaceScript faceScript; // reference in Inspector

    void Start() {
        for (int i = 0; i < buttonList.Count; i++) {
            int index = i;

            // Label button with enum name
            buttonTextList[i].text = ((NPCFace)i).ToString();

            // Hook button click
            buttonList[i].onClick.AddListener(() => CheckCorrectness(index));
        }
    }

    void CheckCorrectness(int index) {
        // Compare button index to current target index
        if (index == GetCurrentIndex()) {
            faceScript.correctFace = true;
            Debug.Log("Correct button pressed");
        } else {
            Debug.Log("Wrong button");
        }
    }

    // Access currIndex WITHOUT modifying RandomFaceScript
    int GetCurrentIndex() {
        var field = typeof(RandomFaceScript)
            .GetField("currIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return (int)field.GetValue(faceScript);
    }
}

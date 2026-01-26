using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class RandomFaceScript : MonoBehaviour {
    public TMP_Text textUI;
    public Button startButton;
    public Button changeFace;
    public UDPExpressionReceiver expresionReceiver;

    bool correctFace = false;
    int currIndex;

    public enum NPCFace {
        Happy,
        Surprised,
        Sad
    };

    void Start() {
        startButton.onClick.AddListener(StartFaceMission);
        //changeFace.onClick.AddListener(ChangeEmotion);
    }

    void Update() {
        // check if user is doing same emotion
        if (expresionReceiver.expression == currIndex) {
            correctFace = true;
        }
    }

    void ChangeEmotion() {
        correctFace = true;
    }

    void StartFaceMission() {
        startButton.gameObject.SetActive(false);
        StartCoroutine(FaceMissionCoroutine());
    }


    IEnumerator FaceMissionCoroutine() {
        int emotionCount = System.Enum.GetValues(typeof(NPCFace)).Length;

        for (int index = 0; index < emotionCount; index++) {
            // Show the current emotion
            textUI.text = ((NPCFace)index).ToString();
            currIndex = index;
            // Wait until the player presses the correct emotion
            correctFace = false;
            while (!correctFace) {
                yield return null; // wait for next frame
            }

            // Correct face detected
            textUI.text = "Good Job!";
            yield return new WaitForSeconds(2f); // wait 2 seconds before next emotion
        }

        // Mission finished
        textUI.text = "All done!";
        startButton.gameObject.SetActive(true); // optional: show button again
    }
}
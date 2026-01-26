using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // dictionary that holds current dialogue nodes by npc IDs
    private Dictionary<string, DialogueNode> currentNodes = new Dictionary<string, DialogueNode>();
    [SerializeField]
    private QuestManager questManager;
    [SerializeField]
    private ReputationManager reputationManager;

    // ui components
    [SerializeField]
    private GameObject dialogueCanvas;
    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private List<(Button button, TMP_Text buttonText)> choicesButtons;

    void Awake()
    {
        // initialize list of choice buttons and their text components
        choicesButtons = new List<(Button, TMP_Text)>();

        // get all buttons in children of dialogue canvas
        Button[] buttons = dialogueCanvas.GetComponentsInChildren<Button>();

        // get text components for each button and add both to list
        foreach (Button btn in buttons)
        {
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
            if (txt != null)
            {
                choicesButtons.Add((btn, txt));
            }
        }
    }

    public void StartDialogue(string npcID, DialogueNode startNode)
    {
        // get current node for the npc
        DialogueNode currentNode = GetCurrentNode(npcID, startNode);

        // populate UI with current node data
        dialogueText.text = currentNode.dialogueText;

        int buttonIndex = 0;
        for (int i = 0; i < currentNode.choices.Count; i++)
        {
            // check if reputation requirement is met
            if (reputationManager.GetReputation(npcID) < currentNode.choices[i].requiredRep)
            {
                // hide button if requirement not met
                choicesButtons[buttonIndex].button.gameObject.SetActive(false);
                continue;
            }
            choicesButtons[buttonIndex].buttonText.text = currentNode.choices[i].choiceText;
            choicesButtons[buttonIndex].button.gameObject.SetActive(true);
            choicesButtons[buttonIndex].button.onClick.RemoveAllListeners();

            int choiceIndex = i; // capture index for closure

            AddListenerToChoice(choiceIndex, npcID, currentNode.choices[choiceIndex].actions);

            buttonIndex++;
        }

        // hide unused buttons
        for (int i = buttonIndex; i < 4; i++)
        {
            choicesButtons[i].button.gameObject.SetActive(false);
        }

        // show dialogue canvas
        dialogueCanvas.SetActive(true);
    }

    public void EndDialogue()
    {
        // hide dialogue canvas
        dialogueCanvas.SetActive(false);
    }

    public DialogueNode GetCurrentNode(string npcID, DialogueNode startNode)
    {
        if (!currentNodes.ContainsKey(npcID))
            currentNodes[npcID] = startNode;

        return currentNodes[npcID];
    }

    public void AddListenerToChoice(int buttonIndex, string npcID, DialogueChoiceAction actions)
    {
        // check for which choice action to perform
        // if statements because IDK if we want buttons to do multiple actions yet
        if ((actions & DialogueChoiceAction.Dialogue) != 0)
        {
            choicesButtons[buttonIndex].button.onClick.AddListener(() =>
            {
                    // update current node based on choice selected
                    currentNodes[npcID] = currentNodes[npcID].choices[buttonIndex].nextNode;

                    // restart dialogue with updated node
                    StartDialogue(npcID, currentNodes[npcID]);
            });
        }

        if ((actions & DialogueChoiceAction.StartQuest) != 0)
        {
            choicesButtons[buttonIndex].button.onClick.AddListener(() =>
            {
                questManager.StartQuest(currentNodes[npcID].choices[buttonIndex].questData);
            });
        }

        if ((actions & DialogueChoiceAction.CheckQuest) != 0)
        {
            choicesButtons[buttonIndex].button.onClick.AddListener(() =>
            {
                Debug.Log($"Checking quest for NPC {npcID}");
                // Implement quest checking logic here
            });
        }

        if ((actions & DialogueChoiceAction.EndDialogue) != 0)
        {
            choicesButtons[buttonIndex].button.onClick.AddListener(() =>
            {
                EndDialogue();
            });
        }
        if ((actions & DialogueChoiceAction.UpdateReputation) != 0)
        {
            choicesButtons[buttonIndex].button.onClick.AddListener(() =>
            {
                reputationManager.UpdateReputation(npcID, currentNodes[npcID].choices[buttonIndex].repChange);
            });
        }
    }
}

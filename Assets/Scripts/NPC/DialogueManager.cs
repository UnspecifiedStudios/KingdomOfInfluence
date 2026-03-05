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

    // player component
    private PlayerMovement playerMvt;
    // player combat component
    private PlayerCombat playerCbt;
    // camera component
    private OrbitCamera playerCam;


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

        playerMvt = gameObject.GetComponent<PlayerMovement>();
        playerCbt = gameObject.GetComponent<PlayerCombat>();
        playerCam = playerCbt.playerObjRefs.cameraReference.GetComponent<OrbitCamera>();
    }

    public void StartDialogue(string npcID, DialogueNode startNode)
    {
        // get current node for the npc
        
        
        DialogueNode currentNode = GetCurrentNode(npcID, startNode);

        // disable player mechanics
        HandlePlayerMechanicPrevention(startNode.lockingMechanisms);

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


                DialogueChoice redirectNode = currentNodes[npcID].choices[buttonIndex];

                // if conditional met, go to new node
                if (redirectNode.branchConditional?.questsToCheck?.Count > 0)
                {
                    Debug.Log("Quest Check List exists. Beginning Check.");
                    bool andConditionalCheck = true;
                    foreach (QuestData questCheck in redirectNode.branchConditional.questsToCheck)
                    {
                        Debug.Log($"Checking Quest with ID: {questCheck.questID}");
                        if (!questManager.activeQuests.ContainsKey(questCheck.questID))
                        {
                            Debug.Log("Quest doesn't exist. Abort.");
                            andConditionalCheck = false;
                            break;
                        }
                        if (questManager.activeQuests[questCheck.questID].status != QuestStatus.Completed)
                        {
                            Debug.Log("Quest is not complete. Abort.");
                            andConditionalCheck = false;
                            break;
                        }
                        else
                        {
                            Debug.Log("Quest is completed!");
                        }
                    }

                    Debug.Log("Checking Conditional...");
                    if (andConditionalCheck)
                    {
                        Debug.Log("Quest List identified as Completed. Assigning Next dialogue node.");
                        currentNodes[npcID] = redirectNode.branchConditional.dialogueToBranchTo;
                    }
                    else
                    {
                        Debug.Log("Quest List identified as INcomplete. Resuming Normal operation.");
                        currentNodes[npcID] = redirectNode.nextNode;
                    }
                }
                else
                {
                    currentNodes[npcID] = redirectNode.nextNode;
                }

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
                HandlePlayerMechanicPrevention(DialogueNode.PlayerLockMechanism.None);
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

    private void HandlePlayerMechanicPrevention(DialogueNode.PlayerLockMechanism lockMech)
    {
        playerCam.mouseMovementDisabled = (lockMech & DialogueNode.PlayerLockMechanism.Camera) != 0;
        playerMvt.lockMovement = (lockMech & DialogueNode.PlayerLockMechanism.Movement) != 0;
        playerCbt.disableAttacking = (lockMech & DialogueNode.PlayerLockMechanism.Attacks) != 0;
    }
}

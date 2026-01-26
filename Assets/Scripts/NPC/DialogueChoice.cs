using UnityEngine;

[System.Flags]
public enum DialogueChoiceAction
{
    None            = 0,
    Dialogue        = 1 << 0,
    StartQuest      = 1 << 1,
    CheckQuest      = 1 << 2,
    EndDialogue     = 1 << 3,
    UpdateReputation     = 1 << 4,
}

[System.Serializable]
public class DialogueChoice
{
    [Tooltip("The dialogue text that players see on button")]
    public string choiceText;
    [Tooltip("Required reputation to display this choice")]
    public int requiredRep;
    [Tooltip("What actions to perform when this choice is selected")]
    public DialogueChoiceAction actions;
    [Tooltip("The quest associated with this choice")]
    public QuestData questData;
    [Tooltip("The reputation change when this choice is selected +/-")]
    public int repChange;
    [Tooltip("The dialogue node to transition to when this choice is selected (if any)")]
    public DialogueNode nextNode;
}


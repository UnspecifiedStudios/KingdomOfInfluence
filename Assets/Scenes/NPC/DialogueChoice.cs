using UnityEngine;

[System.Flags]
public enum DialogueChoiceAction
{
    None            = 0,
    Dialogue        = 1 << 0,
    StartQuest      = 1 << 1,
    CheckQuest      = 1 << 2,
    EndDialogue     = 1 << 3,
}

[System.Serializable]
public class DialogueChoice
{
    [Tooltip("The dialogue text that players see on button")]
    public string choiceText;
    [Tooltip("What actions to perform when this choice is selected")]
    public DialogueChoiceAction actions;
    [Tooltip("The quest associated with this choice")]
    public QuestData questData;
    [Tooltip("The dialogue node to transition to when this choice is selected (if any)")]
    public DialogueNode nextNode;
}


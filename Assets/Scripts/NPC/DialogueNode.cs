using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Scriptable Objects/DialogueNode")]
public class DialogueNode : ScriptableObject
{   
    [System.Flags]
    public enum PlayerLockMechanism
    {
        None        = 0,
        Camera      = 1 << 0,
        Movement    = 1 << 1,
        Attacks     = 1 << 2,
    }

    public string speakerName;

    [TextArea(3, 6)]
    public string dialogueText;

    public PlayerLockMechanism lockingMechanisms;

    public List<DialogueChoice> choices;
}

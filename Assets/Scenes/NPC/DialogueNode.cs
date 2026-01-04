using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Scriptable Objects/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    public string speakerName;

    [TextArea(3, 6)]
    public string dialogueText;

    public List<DialogueChoice> choices;
}

using UnityEngine;

// scriptable object to hold quest data
[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questName;
    [TextArea(3, 6)]
    public string questDescription;
    public int requiredProgress;
}

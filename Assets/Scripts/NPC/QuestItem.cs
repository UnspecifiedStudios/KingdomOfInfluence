using TMPro;
using UnityEngine;

// UI component for displaying a quest in the quest log
public class QuestItem : MonoBehaviour
{
    private QuestData questData;

    [SerializeField]
    private TMP_Text questName;
    [SerializeField]
    private TMP_Text questDescription;

    public void Initialize(QuestRuntimeData quest)
    {
        questData = quest.questData;
        questName.text = questData.questName;
        questDescription.text = questData.questDescription;
    }
}

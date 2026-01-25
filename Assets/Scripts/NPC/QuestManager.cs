using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    // quests
    public Dictionary<string, QuestRuntimeData> activeQuests = new Dictionary<string, QuestRuntimeData>();

    // ui components
    [SerializeField]
    private GameObject questUI;
    [SerializeField]
    private GameObject questList;
    [SerializeField]
    private GameObject questItemPrefab;

    public void OnQuest(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            questUI.SetActive(!questUI.activeSelf);
        }
    }

    public void StartQuest(QuestData questData)
    {
        // create new quest item
        GameObject questItem = Instantiate(questItemPrefab, questList.transform);
        QuestRuntimeData newQuest = new QuestRuntimeData
        {
            questData = questData,
            status = QuestStatus.Active,
            currentProgress = 0,
            requiredProgress = questData.requiredProgress
        };

        // Set the quest item's text or other properties here
        questItem.GetComponent<QuestItem>().Initialize(newQuest);

        // save quest to active quests
        if (!activeQuests.ContainsKey(questData.questID))
        {
            activeQuests.Add(questData.questID, newQuest);
        }
    }

    public void UpdateQuestProgress(string questID)
    {
        QuestRuntimeData quest = activeQuests[questID];
        quest.currentProgress += 1;
        if (quest.currentProgress >= quest.requiredProgress)
        {
            CompleteQuest(questID);
        }
    }

    public void CompleteQuest(string questID)
    {
        activeQuests[questID].status = QuestStatus.Completed;
        Debug.Log($"Quest {activeQuests[questID].questData.questName} marked as completed.");
    }
}

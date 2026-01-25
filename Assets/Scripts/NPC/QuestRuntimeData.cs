public enum QuestStatus
{
    Inactive,
    Active,
    Completed,
    Failed
}

[System.Serializable]
public class QuestRuntimeData
{
    public QuestData questData;
    public QuestStatus status;
    public int currentProgress;
    public int requiredProgress;
}

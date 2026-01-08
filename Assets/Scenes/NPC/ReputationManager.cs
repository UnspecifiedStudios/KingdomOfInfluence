using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    // npc reputation data
    private Dictionary<string, int> reputations = new Dictionary<string, int>();

    // get current reputation for an npc
    // if npc not found, initialize with default reputation of 0
    public int GetReputation(string npcID)
    {
        if (reputations.ContainsKey(npcID))
        {
            return reputations[npcID];
        }

        reputations[npcID] = 0;
        return 0; // default reputation
    }

    // update reputation for an npc by a specified amount
    public void UpdateReputation(string npcID, int amount)
    {
        if (reputations.ContainsKey(npcID))
        {
            reputations[npcID] += amount;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyProgression : MonoBehaviour
{
    private GlobalStateManager_ GSM;

    private void Start()
    {
        GSM = GlobalStateManager_.Instance;
        
    }

    // Enables phone functions
    // Phone itself is only enabled if phoneEnabled is true, which is activated from NPC script when talking to Anna
    public void TalkCount(string npcName)
    {
        
        if (!GSM.GetBool(npcName) && 
            npcName != "partyTalkedToBen" &&
            npcName != "partyTalkedToAnna" &&
            npcName != "partyTalkedToGiorgia")
        {
            var partyTalkCount = GSM.GetInt("partyTalkCount");
            GSM.ChangeInt("partyTalkCount", 1);
        }
        
        GSM.SetBool(npcName, true);
        
        if (GSM.GetInt("partyTalkCount") <= 2 || !GSM.GetBool("partyTalkedToBen") ||
            !GSM.GetBool("partyTalkedToAnna") || !GSM.GetBool("partyTalkedToGiorgia")) return;
        GSM.SetBool("phoneMapEnabled", true);
        GSM.SetBool("mapLocation_Town_Hall_unlocked", true);
        GSM.SetBool("mapLocation_Park_unlocked", true);

    }
}

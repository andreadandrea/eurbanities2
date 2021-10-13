using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStart_ : MonoBehaviour
{
    public string wantedLine = "";
    public string altWantedLine = "";
    public string file = "";
    public string boolCheck = "";
    public bool useRandomLines = false;
    public string[] randomLines;
    GlobalStateManager_ GSM = GlobalStateManager_.Instance;
    TextSystem_ textSystem => TextSystem_.textSystem;

    public void StartDialogue()
    {
        textSystem.textBoard.SetActive(true);
        if (useRandomLines)
        {
            textSystem.ReadFileFor(file, randomLines[Random.Range(0, randomLines.Length)]);
            return;
        }
        if (boolCheck != "" && GSM.GetBool(boolCheck))
            textSystem.ReadFileFor(file, altWantedLine);
        else 
            textSystem.ReadFileFor(file, wantedLine);
    }
}

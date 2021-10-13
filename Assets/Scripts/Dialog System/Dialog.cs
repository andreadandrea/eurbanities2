using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public string wantedLine = "";
    public string altWantedLine = "";
    public string file = "";
    public string boolCheck = "";
    public bool useRandomLines = false;
    public string[] randomLines;
}
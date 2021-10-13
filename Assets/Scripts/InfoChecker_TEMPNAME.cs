using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
[Serializable]
public class InfoChecker_TEMPNAME : MonoBehaviour
{
    public static InfoChecker_TEMPNAME Ic;
    Dictionary<string, bool> boolChecks = new Dictionary<string, bool>();
    public void Start()
    {
        if (Ic == null) Ic = this;
        else Destroy(gameObject);
        StartCoroutine(UpdateBoolDictionary());
    }
    public bool CheckState(string check)
    {
        return boolChecks[check];
    }
    public IEnumerator UpdateBoolDictionary()
    {
        //Start saving symbol
        if (File.Exists($"{Application.dataPath}/SettingFiles/boolChecks.txt"))
        {
            boolChecks.Clear();
            using (StreamReader stream = new StreamReader($"{Application.dataPath}/SettingFiles/boolChecks.txt"))
            {
                string line = stream.ReadLine();
                while (line != null/* || line == ""*/)
                {
                    yield return new WaitForFixedUpdate();
                    string g_string = "";
                    string boll = "";
                    bool boolPart = false;
                    foreach (char character in line)
                    {
                        if (character == ' ') continue;
                        else if (character == '=')
                        {
                            boolPart = true;
                            continue;
                        }
                        if (boolPart)
                            boll += character;
                        else
                            g_string += character;
                    }
                    boolChecks.Add(g_string, bool.Parse(boll));
                    line = stream.ReadLine();
                }
            }
        }
        else
        {
            File.Create($"{Application.dataPath}/SettingFiles/boolChecks.txt");
            Debug.Log("File not found, creating one now");
        }
        //stop saving symol
    }
    public void AddBoolToText(string variable, bool boolState)
    {
        File.Create($"{Application.dataPath}/SettingFiles/boolChecks.txt");
    }
}

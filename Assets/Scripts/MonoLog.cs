using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "New Monolog", menuName = "Monolog")]
public class MonoLog : ScriptableObject
{
    public string randomFileName = "";
    public string boolCheck = "";
    public string altRandomFileName = "";
    public string importantFileName = "";
    public string importantBoolCheck = "";
    public string altImportantFileName = "";
    [HideInInspector] public List<string> randomText = new List<string>();
    [HideInInspector] public List<string> importantText = new List<string>();
    public bool initiated = false;
    GlobalStateManager_ GSM = GlobalStateManager_.Instance;
    Settings_ settings => Settings_.settings;
    
    
    public void Initiate()
    {
        UpdateLinesAvailable();
        AddToImportant();
        initiated = true;
    }
    private void UpdateLinesAvailable()
    {
        Debug.Log("d�ner");
        string path = "";
        if (altRandomFileName != "" && GSM.GetBool(boolCheck)) path = $"{settings.Datapath}/Storyline/Text Files/{settings.GetCurrentLanguageFolder()}/Misc/Random_mono/{altRandomFileName}.txt";
        else if (randomFileName != "") path = $"{settings.Datapath}/Storyline/Text Files/{settings.GetCurrentLanguageFolder()}/Misc/Random_mono/{randomFileName}.txt";
        Debug.Log(path + "_" + randomFileName);
        if (path == "") return;
        if (!File.Exists(path))
        {
            Debug.LogError("Path was not found or file path was typed wrong");
            return;
        }
        using StreamReader stream = new StreamReader(path);
        string line = stream.ReadLine();
        while (line != null)
        {
            if (line != "") randomText.Add(line);
            line = stream.ReadLine();
        }
    }
    private void AddToImportant()
    {
        string path = "";
        if (altImportantFileName != "" && GSM.GetBool(boolCheck)) path = $"{settings.Datapath}/Eurbanities2-copy/{settings.GetCurrentLanguageFolder()}/Misc/Important_mono/{altImportantFileName}.txt";
        else if (importantFileName != "") path = $"{settings.Datapath}/Eurbanities2-copy/{settings.GetCurrentLanguageFolder()}/Misc/Important_mono/{importantFileName}.txt";
        if (path == "") return;
        if (!File.Exists(path))
        {
            Debug.LogError("Path was not found or file path was typed wrong");
            return;
        }
        using StreamReader stream = new StreamReader(path);
        string line = stream.ReadLine();
        while (line != null)
        {
            if (line != "") importantText.Add(line);
            line = stream.ReadLine();
        }
    }
}

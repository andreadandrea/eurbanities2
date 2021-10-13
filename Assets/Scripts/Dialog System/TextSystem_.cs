using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;/*
using System.Runtime;
using System.Diagnostics;*/

public class TextSystem_ : MonoBehaviour
{

    FunnyTextHandler_ FTH => FunnyTextHandler_.FTH;
    PlayerResponse_ PR => PlayerResponse_.PR;

    //Stopwatch sw = new Stopwatch();

    public GameObject textBoard; //where the text will be instantiated

    public static TextSystem_ textSystem;

    Settings_ settings => Settings_.settings;

    string previousPath = "";
    private void Awake()
    {
        
        textSystem = this;
        previousPath = $"{Application.dataPath}/Storyline/Text Files/BEN VS JESSICA.txt";
        emojiDic = new Dictionary<string, Sprite>()
        {
            {"face_angry",  emojis[0] },
            {"face_happy",  emojis[1] },
            {"face_kekw",   emojis[2] },
            {"face_meh",    emojis[3] },
            {"face_sad",    emojis[4] },
        };
    }
    
    public void ReadFileFor(string path, string wantedLine)
    {
        
        if (!FTH.CanISendNextText()) return;
        if (wantedLine.ToLower() == "#end")
        {
            FTH.ClearText();
            return;
        }
        if (path != "") previousPath = $"{settings.Datapath}/Storyline/Text Files/{settings.GetCurrentLanguageFolder()}/Areas/{settings.GetCurrentAreaFolder()}/{path}.txt";
        
        StreamReader stream;
        try { stream = new StreamReader(previousPath); }
        catch { UnityEngine.Debug.Log("that did not work");  return; } //it will run this code if the "try" code gets an error in some way
        #region temp variables
        bool collectingText = false;
        int skipTheseFixLaterPlease = 4;
        string currentLine = "", name = "", emoji = "", maintext = "", tempString = "", checkForFunctions = "", variable = "NULL";
        List<Option> options = new List<Option>();
        #endregion
        while (currentLine == "" || currentLine.Substring(0, 4) != wantedLine)
        {
            currentLine = stream.ReadLine();
            if (currentLine == null)
            {
                UnityEngine.Debug.Log("Nothing was found");
                return;
            }
        }
        /*bool moreTosee = true;
        int iterations = 0;
        while (moreTosee)
        {
            iterations++; //keeps track of the number of lines that has been checked
            currentLine = stream.ReadLine(); //reads the the next line, it will keep track of the current line by itself

            if (currentLine == null) //checks if you have reached the end of the text file
            {
                UnityEngine.Debug.Log($"these are not the droids you are looking for, the file has {iterations} lines tho");
                return;
            }
            else if (currentLine.Length < 4) continue; //checks if there is less then 4 characters in the curretn line, meaning that there is nothing for you here

            string subString = currentLine.Substring(0, 4);
            if (wantedLine == subString)//this checks if you have found the right line that you were looking for
            {
                //UnityEngine.Debug.Log(iterations != 1 ? $"FOUND IT! :D, it was on the {iterations}th line" : $"FOUND IT! :D, it was on the {iterations}st line");
                moreTosee = false;
            }
        }*/
        //now hopefully i have found the line i need to work with, and need to start parting it up into its different parts
        currentLine = currentLine.Replace("$$", Settings_.settings.PlayerName);
        if (currentLine.Contains("$$")) Debug.Log("shit is here");

        foreach (char character in currentLine)
        {
            if (skipTheseFixLaterPlease > 0)
            {
                skipTheseFixLaterPlease--;
                continue;
            }
            switch (variable)
            {
                case "TEXT":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                    }
                    else maintext += character;
                    continue;
                case "OPTION":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options.Add(new Option { optionNumb = tempString});
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "OPTION_TEXT":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].optionText = tempString;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "OPTION_EXPLANATION":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].optionExplanation = tempString;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "EMOJI":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                    }
                    else emoji += character;
                    continue;
                case "NAME":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                    }
                    else name += character;
                    continue;
                case "BOOLCHECK":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].boolcheck = tempString;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "FAIL_MESSAGE":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].failedText = tempString;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "FLAG_SET":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].flagSet = tempString;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "SWITCH":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].switchingSide = true;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "SCENELOAD":
                    if (character == '|')
                    {
                        collectingText = false;
                        variable = "NULL";
                        checkForFunctions = "";
                        options[options.Count - 1].sceneLoad = tempString;
                        tempString = "";
                    }
                    else tempString += character;
                    continue;
                case "NULL":
                    break;
                default:
                    UnityEngine.Debug.Log($"you are not supose to be here! \"{variable}\"");
                    break;
            }
            if (!collectingText)
            {
                if (character == ' ' || character == '\t') continue;
                if (character == '|')
                {
                    collectingText = true;
                    variable = checkForFunctions;
                    //UnityEngine.Debug.Log(variable);
                }
                else checkForFunctions += character;
            }
        }
        StartCoroutine(FTH.WritheTheStuff(maintext));
        StartCoroutine(PR.WaitForConfirmation(options));
        emojiDic.TryGetValue(emoji.ToLower(), out Sprite sprit);
        if (sprit)
        {
            emojiSlot.color = Color.white;
            emojiSlot.sprite = sprit;
        }
        else emojiSlot.color = Color.clear;
        nameSlot.text = name;
        #region redundantCode
        /*if (options.Count < 1)
          {
              //add a option to just press the "continue" button
              try
              {
                  string nextLineInhere = stream.ReadLine().Substring(0, 4);
                  if (nextLineInhere.Length == 4) StartCoroutine(SpawnOptions(new Option { optionNumb = nextLineInhere, optionText = "Continue", optionExplanation = "" }, 0, new Reality { boolcheck = "true", failedText = "" }));
                  else StartCoroutine(SpawnOptions(new Option { optionNumb = "#end", optionText = "End Convenstaion", optionExplanation = "" }, 0, new Reality { boolcheck = "true", failedText = "" }));
              }
              catch
              {
                  StartCoroutine(SpawnOptions(new Option
                  { optionNumb = "#end", optionText = "End Convenstaion", optionExplanation = "" },
                      0, new Reality { boolcheck = "true", failedText = "" }));
              }
          }
          for (int i = 0; i < options.Count; i++)
          {


              try
              {
                  realityCheck.TryGetValue(i, out Reality check);
                  StartCoroutine(SpawnOptions(options[i], i, check != null ? check : new Reality { boolcheck = "true", failedText = "" }));
                  if (check != null) UnityEngine.Debug.Log(check.boolcheck + check.failedText);
              }
              catch (System.Exception e)
              {
                  UnityEngine.Debug.Log("Someting is wrong...");
                  UnityEngine.Debug.Log(e);
              }//changeing so that i can add all the options to the new thang
          }*/
        #endregion
        //UnityEngine.Debug.Log($"name = {name}, emoji = {emoji}, text = {maintext}");
        stream.Close();
        //UnityEngine.Debug.Log("it took " + sw.Elapsed);
    }
    /*IEnumerator SpawnOptions(Option option, int i)
    {
        yield return new WaitForFixedUpdate();
        //check for the bool that is needed to press the button, else make it grayed out
        GameObject button = Instantiate(optionButton, target.transform);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(345, -278 - i * 80);
        button.GetComponent<OptionFuntion_>().wantedLine = option.optionNumb;
        if (!InfoChecker_TEMPNAME.Ic.CheckState(option.boolcheck))
        {
            button.GetComponent<Button>().interactable = false; //make it grayed out
            button.GetComponentInChildren<Text>().text = option.failedText;
        }
        else button.GetComponentInChildren<Text>().text = option.optionText;
    }*/
    public class Option
    {
        public string optionNumb = "";
        public string optionText = "";
        public string optionExplanation = "";
        public string boolcheck = "";
        public string failedText = "";
        public string flagSet = "";
        public string sceneLoad = "";
        public bool switchingSide = false;
    }
    [SerializeField] Sprite[] emojis;
    Dictionary<string, Sprite> emojiDic;
    [SerializeField] Image emojiSlot;
    [SerializeField] Text nameSlot;
}

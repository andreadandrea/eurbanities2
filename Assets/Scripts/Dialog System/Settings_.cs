using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_ : MonoBehaviour
{
    readonly static Dictionary<Languages, string> languageFiles = new Dictionary<Languages, string>
    {
        {Languages.Svenska,   "sv_SV"},
        {Languages.English,   "en_EN"},
        {Languages.Deutsche,  "du_DU"},
        {Languages.Magyar,    "ma_MA"},
        {Languages.Românã,    "ro_RO"},
        {Languages.Français,  "fr_FR"},
        {Languages.Polskie,   "po_PO"},
        {Languages.Italiano,  "it_IT"},
    };

    public Languages CurrentLanguage = Languages.English;
    public static Settings_ settings;
    public GameObject introcution;
    public string PlayerName = "null";
    public string Datapath => Application.dataPath;
    public bool AnimatedText = true;

    private void Start()
    {
        StartCoroutine(GameInstruction());
        settings = this;
    }
    public void EndGameInstructions()
    {
        GlobalStateManager_.Instance.SetBool("canMove", true);
        Time.timeScale = 1;
        Destroy(introcution);
    }
    public IEnumerator GameInstruction()
    {
        yield return new WaitUntil(() => GlobalStateManager_.Instance.GetInt("currentScene") == 4);
        yield return new WaitForSecondsRealtime(2); 
        if (!GlobalStateManager_.Instance.GetBool("Game_Introduction"))
        {
            GlobalStateManager_.Instance.SetBool("canMove", false);
            GlobalStateManager_.Instance.SetBool("Game_Introduction", true);
            introcution.SetActive(true);
        }
        else Destroy(introcution);
    }

    public string GetCurrentLanguageFolder()
    {
        return languageFiles[CurrentLanguage];
    }
    public string GetCurrentAreaFolder()
    {
        return Enum.GetName(typeof(Areas), GlobalStateManager_.Instance.GetInt("currentScene"));
    }
    
    public void SetLanguage(Languages language)
    {
        CurrentLanguage = language;
        //Make a function to change all the languages when you a: klick this or b: go out of the settings menu
    }
}
public enum Languages
{
    Svenska,
    English,
    Deutsche,
    Magyar,
    Românã,
    Français,
    Polskie,
    Italiano,
}
public enum Areas
{
    MasterScene = 0,
    MobileScene = 1,
    MainStreet = 2,
    FlowerShop = 3,
    ParkParty = 4,
    ParkAfterParty = 5,
    TownHall = 6,
    TownHallInside = 7,
    Garden = 8,
    CommunityCenter = 9,
    CommunityCenterInside = 10
}

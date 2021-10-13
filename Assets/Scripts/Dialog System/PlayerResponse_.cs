using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerResponse_ : MonoBehaviour
{
    public delegate void yeet();
    public yeet SwitchSet;
    public static PlayerResponse_ PR;
    public delegate void Initiate();
    public Initiate StartInitiation;
    private FunnyTextHandler_ fth => FunnyTextHandler_.FTH;
    private TextSystem_ textSystem => TextSystem_.textSystem;
    [SerializeField] private Text textArea;
    [SerializeField] private Button previous, next;
    [SerializeField] private int currentOption = 0;
    [SerializeField] private Toggle newToggle;
    private List<Toggle> options = new List<Toggle>();
    GlobalStateManager_ GSM = GlobalStateManager_.Instance; 
    private bool done = false;

    public static event Action CloseDialog;

    private void Awake()
    {
        PR = this;
    }
    public List<TextSystem_.Option> optionList = new List<TextSystem_.Option>();
    public IEnumerator WaitForConfirmation(List<TextSystem_.Option> options) //start when the text system starts to write,
    {                                       //so the options dose not get available untill the text is done writing
        optionList = options;
        yield return new WaitUntil(() => !fth.occupied);
        FixOptions();
    }
    private void FixOptions()
    {
        int shit = optionList.Count;
        if (shit > 1) //needs more optoins to sift trough
        {
            next.gameObject.SetActive(true);
            previous.gameObject.SetActive(true);
            for (int i = 0; i < shit; i++)
            {
                Toggle toggle = Instantiate(newToggle, transform);
                toggle.transform.localPosition = new Vector2(((-shit / 2) + i) * 25, 30);
                options.Add(toggle);
            }
            textArea.text = optionList[0].optionText;
            options[0].isOn = true;
        }
        else if (shit == 1) textArea.text = optionList[0].optionText;
        else done = true;
    }
    public void ChangeOpion(bool plus)
    {
        if (!plus && currentOption < 1) return;
        else if (plus && currentOption == optionList.Count - 1) return;
        options[currentOption].isOn = false;
        currentOption = Mathf.Clamp(currentOption += plus ? 1 : -1, 0, optionList.Count);
        if (GSM.GetBool(optionList[currentOption].boolcheck))
             textArea.text = optionList[currentOption].optionText;
        else 
            textArea.text = optionList[currentOption].failedText;
        options[currentOption].isOn = true;

    }
    public void Respond()
    {
        if (!FunnyTextHandler_.FTH.CanISendNextText()) return;
        if (optionList.Count > 0)
        {
            if (!GSM.GetBool(optionList[currentOption].boolcheck)) return;
            //StateSystem_.MarkAFlag(optionList[currentOption].flagSet); previous
            if (optionList[currentOption].flagSet != "")
                GSM.SetBool(optionList[currentOption].flagSet, true); // New
        }
        if (optionList[currentOption].sceneLoad  != "") SceneManager.LoadSceneAsync(optionList[currentOption].sceneLoad, LoadSceneMode.Additive);
        if (done || optionList.Count < 1 || optionList[currentOption].optionNumb.ToLower() == "#end")
        {
            ClearDown();
            transform.parent.gameObject.SetActive(false);
            CloseDialog?.Invoke();
            done = false;
            CheckThis(optionList[currentOption].switchingSide);
            currentOption = 0;
            return;
        }
        textSystem.ReadFileFor("", optionList[currentOption].optionNumb);
        //Debug.Log($"{optionList[currentOption].optionNumb} witch is the {currentOption + 1}rd option, out of {optionList.Count}");
        ClearDown();
        currentOption = 0;
    }
    private void ClearDown()
    {
        next.gameObject.SetActive(false);
        previous.gameObject.SetActive(false);
        //Debug.Log(options.Count);
        if (options.Count > 0)
            foreach (Toggle toggle in options)
            {
                if (toggle == null) continue;
                Destroy(toggle.gameObject);
            }
        options = new List<Toggle>();
        textArea.text = "";
    }
    private void CheckThis(bool var)
    {
        if (var)
        {
            StartInitiation?.Invoke();
        }
        Delegate.RemoveAll(StartInitiation, null);
    }
}

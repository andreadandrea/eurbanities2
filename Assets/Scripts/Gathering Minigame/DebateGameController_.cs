using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebateGameController_ : MonoBehaviour
{
    [SerializeField] Text[] statsText;
    int[] argumentStats = { 0, 0, 0 };
    [SerializeField] Slider approvalSlider;
    [SerializeField] int roundAmount = 3;
    [SerializeField] Text resultText;
    int currentRound;
    int currentApproval;
    int maxApproval;
    int minApproval;
    int argumentIndex;
    bool rerollUsed = false;
    [SerializeField] Text currentApprovalText;
    [FMODUnity.EventRef] [SerializeField] private string yaySound, booSound, winSound, loseSound; 


    NPC_[] NPCs;
    [SerializeField] private StateMachine state = StateMachine.ChooseTopic;

    [SerializeField] GameObject debateCanvas;
    [SerializeField] GameObject npcs;
    [SerializeField] GameObject rerollButton;
    [SerializeField] GameObject resultCanvas;

    [SerializeField] GameObject[] topicResultUI;
    [SerializeField] Sprite[] topicResultSprite;
    [SerializeField] Text currentTopicText;
    [SerializeField] Text[] topicsText;
    public List<string> topics = new List<string>();
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject chooseTopicCanvas;
    int currentTopicIndex = 0;
    public HighlightButton_[] highlightButtons;
    public int GetRandomArgumentIndex()
    {
        return Random.Range(0, 3);
    }

    public bool ChooseTopic(string topic)
    {



        if (topics.Count < 3 && !topics.Contains(topic))
        {
            topics.Add(topic);
            // currentTopicText.text = "We need " + topics[currentTopicIndex] + " because...";
            return true;


        }
        else
        {
            topics.Remove(topic);
            return false;
        }
    }




    private void TopicResultUI()
    {
        for (int i = 0; i < topicResultUI.Length; i++)
        {
            if(topicResultUI[i].GetComponent<Image>().sprite == null) topicResultUI[i].GetComponent<Image>().color =new Color(0,0,0,0);
            else topicResultUI[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
           
    }

    public void ResetGame()
    {
        rerollUsed = false;
        continueButton.SetActive(false);
        SwitchState(StateMachine.ChooseTopic);
        currentTopicIndex = 0;
        topics.Clear();
        for (int i = 0; i < topicResultUI.Length; i++)
        {
            topicResultUI[i].GetComponent<Image>().sprite = null;
        }
        foreach (HighlightButton_ highlightButton in highlightButtons)
        {
            highlightButton.DisableButton();
            highlightButton.ChangeColor(false);
        }
        TopicResultUI();
    }



    public void ResetDebate()
    {
        currentRound = 0;
        currentApproval = 0;
        foreach (NPC_ npc in NPCs)
        {
            npc.StartingOpinion();
            RerollArguments(true);
        }
        ArgumentStatistics();

    }
    public void NextTopic()
    {
        ResetDebate();
        if (currentTopicIndex < 2)
        {
            currentTopicIndex++;
            SwitchState(StateMachine.Debate);
            currentTopicText.text = "We need " + topics[currentTopicIndex] + " because...";


        }
        else
        {

            ResetGame();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    public void RerollArguments(bool isNextTopic)
    {
        if (isNextTopic)
        {

            foreach (NPC_ npc in NPCs)
            {
                npc.SetRandomArgument();
            }
            ArgumentStatistics();


        }
        else if (!rerollUsed)
        {
            foreach (NPC_ npc in NPCs)
            {
                npc.SetRandomArgument();
            }
            ArgumentStatistics();
            rerollUsed = true;

        }



    }
    public int CompareResult(int npcArgument)
    {

        if (npcArgument == argumentIndex) return 0;
        else if (npcArgument == argumentIndex - 1 || (npcArgument == 2 && argumentIndex == 0)) return 1;
        else return -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        //currentRound = 1;
        SwitchState(StateMachine.ChooseTopic);
        chooseTopicCanvas.SetActive(state == StateMachine.ChooseTopic);
        resultCanvas.SetActive(false);

        highlightButtons = FindObjectsOfType<HighlightButton_>();
        NPCs = FindObjectsOfType<NPC_>();
        foreach (NPC_ npc in NPCs)
        {
            maxApproval += 1;
            minApproval -= 1;
        }
        approvalSlider.maxValue = maxApproval;
        approvalSlider.minValue = minApproval;

        //approvalSlider.enabled = false;
        ArgumentStatistics();
        TopicResultUI();

    }
    public void ChooseArgument(int index)
    {

        if (currentRound < roundAmount)
        {
            currentRound++;
            currentApproval = 0;
            argumentIndex = index;
            foreach (NPC_ npc in NPCs)
            {

                npc.CompareArgumentButton();
                currentApproval += npc.Vote();
                currentApprovalText.text = "Approval: " + currentApproval.ToString();
            }
            ArgumentStatistics();
            if (currentRound >= roundAmount)
            {
                CheckResult();

            }
        }
        else
        {


        }



    }

    public void ArgumentStatistics()
    {
        for (int i = 0; i < argumentStats.Length; i++)
        {
            argumentStats[i] = 0;
        }

        foreach (NPC_ npc in NPCs)
        {

            if (npc.GetArgument() == 0) argumentStats[0]++;
            else if (npc.GetArgument() == 1) argumentStats[1]++;
            else argumentStats[2]++;
        }
        for (int i = 0; i < statsText.Length; i++)
        {
            statsText[i].text = argumentStats[i].ToString();
        }
    }
    public enum StateMachine
    {
        ChooseTopic, Debate, Result
    }

    public void SwitchState(StateMachine newState)
    {
        state = newState;
    }
    public void SwitchStateButton(int stateIndex)
    {
        if (stateIndex == 1) state = StateMachine.ChooseTopic;
        else if (stateIndex == 2) state = StateMachine.Debate;
        else if (stateIndex == 3) state = StateMachine.Result;
    }




    private void CheckResult()
    {
       
        // TopicsResultList();
        if (currentApproval > 0)
        {
            topicResultUI[currentTopicIndex].GetComponent<Image>().sprite = topicResultSprite[0];
            resultText.text = "APPROVED";
            FMODUnity.RuntimeManager.PlayOneShot(yaySound, transform.position);
            FMODUnity.RuntimeManager.PlayOneShot(winSound, transform.position);
        }
        else if (currentApproval <= 0)
        {
            topicResultUI[currentTopicIndex].GetComponent<Image>().sprite = topicResultSprite[1];
            resultText.text = "DECLINED";
            FMODUnity.RuntimeManager.PlayOneShot(booSound, transform.position);
            FMODUnity.RuntimeManager.PlayOneShot(loseSound, transform.position);
        }

        TopicResultUI();


    }
    private void Update()
    {




        switch (state)
        {
            case StateMachine.ChooseTopic:

                break;
            case StateMachine.Debate:

                rerollButton.SetActive(!rerollUsed);
                currentTopicText.text = "We need " + topics[currentTopicIndex].ToLower() + " because...";

                for (int i = 0; i < topics.Count; i++)
                {

                    topicsText[i].text = topics[i];
                }
                break;

            case StateMachine.Result:
                break;
        }


        /*for (int i = 0; i < statsText.Length; i++)
        {
            statsText[i].text = argumentStats[i].ToString();
        }
        */
        approvalSlider.value = currentApproval;


        //result UI
        resultCanvas.SetActive(state == StateMachine.Result);
        //currentApprovalText.enabled = state == StateMachine.Result;


        //Debate UI
        debateCanvas.SetActive(state == StateMachine.Debate || state == StateMachine.Result);
        npcs.SetActive(state == StateMachine.Debate || state == StateMachine.Result);

        //Chose Topic UI
        chooseTopicCanvas.SetActive(state == StateMachine.ChooseTopic);
        continueButton.SetActive(topics.Count >= 3);


        //if(topics[currentTopicIndex] != null) topicText.text = "We need " + topics[currentTopicIndex] + " becuase...";

        if (currentRound >= roundAmount)
        {
            SwitchState(StateMachine.Result);
        }
    }






}








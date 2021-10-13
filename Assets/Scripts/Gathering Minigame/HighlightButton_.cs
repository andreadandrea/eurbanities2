using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighlightButton_ : MonoBehaviour
{
    [SerializeField] Color colorHighlighted, colorNormal;
    Image buttonImage;
    Button button;
    bool isUsed = false;
    [SerializeField]string topic;
    DebateGameController_ gameController;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        //button.onClick.AddListener(ChangeColor);
        button.onClick.AddListener(ChooseTopicButton);
        gameController = FindObjectOfType<DebateGameController_>();
    }

    private void Update()
    {

    }
    public void DisableButton()
    {
      
        if (isUsed)
        {
        
            this.gameObject.SetActive(false);
        }
    }
    public void ChooseTopicButton()
    {
      //  addTopic = gameController.ChooseTopic(topic);
       
        ChangeColor(gameController.ChooseTopic(topic));
        
        
    }

    public void ChangeColor(bool isAddTopic)
    {


        // isHighlighted = !isHighlighted;
            isUsed = isAddTopic;
            if (isAddTopic) buttonImage.color = colorHighlighted;
            else buttonImage.color = colorNormal;
        
    }



}

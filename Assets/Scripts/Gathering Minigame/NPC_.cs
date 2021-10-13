using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;
public class NPC_ : MonoBehaviour
{

    [SerializeField] Sprite[] faceM, faceF;
    [SerializeField] GameObject[] sillueteSex;
    [SerializeField] Text yesNoIDC;

    [SerializeField] GameObject emojiObject;
    [SerializeField] Sprite[] emojiSprites;
    SpriteRenderer emojiSpriteRenderer;

    [SerializeField] Sprite[] argumentSprite;
    
    DebateGameController_ argument;
    [SerializeField] Text pointText;
    int argumentIndex;
    int currentOpinionIndex;
    int opinionIndexChange;
    [SerializeField]SpriteRenderer argumentSpriteRenderer;
    
    [SerializeField] Transform FaceTEST;

    string[] faceLabels = { "face", "face_1", "face_2", "face_3" };
    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0, 2);
        GameObject silluete = Instantiate(sillueteSex[i], transform.position, Quaternion.identity);
        silluete.transform.parent = transform;
      
        SpriteResolver spriteResolver = GetComponentInChildren<SpriteResolver>();
        spriteResolver.SetCategoryAndLabel("Face", faceLabels[Random.Range(0, faceLabels.Length)]);
        emojiSpriteRenderer = emojiObject.GetComponent<SpriteRenderer>();
        argument = FindObjectOfType<DebateGameController_>();
        
        
 
        StartingOpinion();

        SetRandomArgument();
        
    }
    public void StartingOpinion()
    {
        currentOpinionIndex = Random.Range(-1, 2);

        pointText.text = currentOpinionIndex.ToString();
    }

    public void SetRandomArgument()
    {

        argumentIndex = argument.GetRandomArgumentIndex();
        argumentSpriteRenderer.sprite = argumentSprite[argumentIndex];
        ChangeEmoji();
    }
    public int Vote()
    {
        if (currentOpinionIndex > 0) return 1;
        else if (currentOpinionIndex < 0) return -1;
        else return 0;
       
    }
    public void CompareArgumentButton()
    {
        opinionIndexChange = argument.CompareResult(argumentIndex);
        if (currentOpinionIndex<2 && currentOpinionIndex>-2) currentOpinionIndex += opinionIndexChange;
        else if(currentOpinionIndex == 2 && opinionIndexChange != 1) currentOpinionIndex += opinionIndexChange;
        else if(currentOpinionIndex ==-2 && opinionIndexChange != -1) currentOpinionIndex += opinionIndexChange;
        
        pointText.text = currentOpinionIndex.ToString();
        argumentIndex = argument.GetRandomArgumentIndex();
        argumentSpriteRenderer.sprite = argumentSprite[argumentIndex];
        
        ChangeEmoji();
    }


    public int GetArgument()
    {
        return argumentIndex;
    }
      
    // Update is called once per frame
    void Update()
    {
        if (currentOpinionIndex > 0) yesNoIDC.text = "YES";
        else if (currentOpinionIndex < 0) yesNoIDC.text = "NO";
        else yesNoIDC.text = "i dont care";
    }

    void ChangeEmoji()
    {
        if (currentOpinionIndex == -2) emojiSpriteRenderer.sprite = emojiSprites[0];
        else if (currentOpinionIndex == -1) emojiSpriteRenderer.sprite = emojiSprites[1];
        else if (currentOpinionIndex == 0) emojiSpriteRenderer.sprite = emojiSprites[2];
        else if (currentOpinionIndex == 1) emojiSpriteRenderer.sprite = emojiSprites[3];
        else if (currentOpinionIndex == 2) emojiSpriteRenderer.sprite = emojiSprites[4];
    }
}

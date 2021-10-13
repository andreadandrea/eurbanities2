using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RandomDialoguePromt_ : MonoBehaviour
{
    [SerializeField] private GameObject pratBubla; 
    private GameObject instanceBubla;
    private readonly Dictionary<Areas, Vector3> speechBubbleOffset = new Dictionary<Areas, Vector3>()
    {
        { Areas.ParkParty, new Vector3(1.5f, 4.5f)},
        { Areas.ParkAfterParty, new Vector3(1.5f, 4.5f)},
        { Areas.CommunityCenter, new Vector3(0, 0)},
        { Areas.FlowerShop, new Vector3(0, 0)}
    };
    bool occupied = false;
    bool important = false;
    int lastOption = -1;

    private GlobalStateManager_ GSM;
    
    private void Start()
    {
        GSM = GlobalStateManager_.Instance;
        Simon_NPC_.MonologStart += Call;
    }
    private void OnDestroy()
    {
        Simon_NPC_.MonologStart -= Call;
    }
    public void Call(GameObject target, MonoLog temp)
    {
        if (important) return;
        if (occupied)
        {
            StopAllCoroutines();
            Destroy(instanceBubla.gameObject);
        }
        //start random text
        if (temp.importantText.Count > 0)
        {
            important = true;
            StartCoroutine(PrintText(target, temp.importantText[0]));
            temp.importantText.RemoveAt(0);
            occupied = true;
        }
        else
        {
            if (temp.randomText.Count < 1) return;
            int textInt = 0;
            if (temp.randomText.Count > 1)
            {
                textInt = Random.Range(0, temp.randomText.Count);
                while (textInt == lastOption)
                {
                    textInt = Random.Range(0, temp.randomText.Count);
                }
            }
            lastOption = textInt;
            StartCoroutine(PrintText(target, temp.randomText[textInt]));
            occupied = true;
        }
    }
    
        //instantiate a box with the text comming in slowly
    private IEnumerator PrintText(GameObject target, string text)
    {
        //int lines = Mathf.CeilToInt(text.Length / 20);
        instanceBubla = Instantiate(pratBubla);
        instanceBubla.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta += new Vector2(Mathf.Clamp(30 * (text.Length - 3), 0, 750), 75 * Mathf.FloorToInt(text.Length / 30));
        instanceBubla.transform.position = target.transform.position + speechBubbleOffset[(Areas) GSM.GetInt("currentScene")];
        Text yeet = instanceBubla.transform.GetChild(0).GetComponentInChildren<Text>();
        float time = 2;
        text.Replace("$$", Settings_.settings.PlayerName);
        foreach (char character in text)
        {
            yeet.text += character;
            time += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSecondsRealtime(time);
        important = false;
        Destroy(instanceBubla.gameObject);
    }
}

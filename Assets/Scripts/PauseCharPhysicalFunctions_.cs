using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseCharPhysicalFunctions_ : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string infoMessage = null;

    public bool hasAMessage = false;

    GameObject panel;

    public List<CharacterPhysicalFunctionScript_> charsToEffect = new List<CharacterPhysicalFunctionScript_>();
    private void Start()
    {
        //panel = GameObject.Find("ExtraInfoPanel").gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasAMessage)
        {
            /*panel.SetActive(true);
            panel.GetComponentInChildren<Text>().text = infoMessage;*/
        }
        for (int i = 0; i < charsToEffect.Count; i++)
        {
            charsToEffect[i].stopDancing = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //panel.SetActive(false);
        for (int i = 0; i < charsToEffect.Count; i++)
        {
            charsToEffect[i].stopDancing = false;
        }
    }
}

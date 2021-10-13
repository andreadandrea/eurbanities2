using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simon_ChangeMoveMethod_ : MonoBehaviour
{
    //[SerializeField] Text clickSwipeText;

    GlobalStateManager_ GSM;
    private void Awake()
    {
        Simon_OpenPhone_.PhoneOpen += OpenPhone;
        Simon_OpenPhone_.PhoneClosed += ClosePhone;
    }
    void Start()
    {
        GSM = GlobalStateManager_.Instance;
       // ChangeText();
    }

    private void OnDestroy()
    {
        Simon_OpenPhone_.PhoneOpen -= OpenPhone;
        Simon_OpenPhone_.PhoneClosed -= ClosePhone;
    }

    private void OpenPhone()
    {
        gameObject.SetActive(false);
    }
    private void ClosePhone()
    {
        gameObject.SetActive(true);
    }
    public void ClickSwipeToggle()
    {
        GSM.ToggleBool("pointAndClick");
        GSM.ToggleBool("swipe");

       // ChangeText();
    }
   /* private void ChangeText()
    {
        if (gameSession.pointAndClick)
        {
            clickSwipeText.text = "Point And Click";
        }
        else
        {
            clickSwipeText.text = "Swipe";
        }
    }*/
    public void ButtonsToggle()
    {
        GSM.ToggleBool("buttons");
    }
}
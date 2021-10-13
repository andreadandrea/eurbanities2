using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simon_OpenPhone_ : MonoBehaviour
{
    public static event Action PhoneOpen;
    public static event Action PhoneClosed;
    Animator animator;
    PhoneAudio_ phoneAudio;
    private GlobalStateManager_ GSM;

    [SerializeField] private Button mapButton;
    

    void Start()
    {
        phoneAudio = GetComponent<PhoneAudio_>();
        animator = GetComponent<Animator>();
        GSM = GlobalStateManager_.Instance;
        
    }
    public void OpenPhone()
    {
        if (GSM.GetBool("inDialog")) return;
        if (GlobalStateManager_.Instance.GetBool("phoneEnabled"))
        {
            GSM.SetBool("canMove", false);
            PhoneOpen?.Invoke();
            animator.SetTrigger("open_phone");
            phoneAudio.phoneOpenSound(true);
        }
        else
        {
            phoneAudio.phoneLockedSound();
        }
        
        // phoneMapEnabled is set in ParkProgression.cs
        mapButton.enabled = GSM.GetBool("phoneMapEnabled");

    }
    public void ClosePhone()
    { 
        PhoneClosed?.Invoke();
        phoneAudio.phoneOpenSound(false);
        GSM.SetBool("canMove", true);
    }
}

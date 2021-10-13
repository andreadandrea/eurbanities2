using System;
using UnityEngine;
using UnityEngine.UI;

public class PinGoalSetup_ : MonoBehaviour
{
    private GlobalStateManager_ GSM;
    private Button btn;
    
    private void Awake()
    {
        GSM = GlobalStateManager_.Instance;

        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => { FindObjectOfType<PinInteraction_>().onClick(transform); });
    }

    private void OnEnable()
    {
        PinInfo_ pinInfo = GetComponent<PinInfo_>();
        pinInfo.unlocked = true; //this should be set to check GSM if the place is unlocked,
                                 //for testing purposes this is just true to enable all places -
                                 //GSM.GetBool($"mapLocation_{pinInfo.scene}_unlocked");
        ColorBlock cb = btn.colors;
        cb.normalColor = pinInfo.unlocked ? new Color(0f, 0.75f, 0f) : new Color(0.75f, 0f, 0f);
        cb.selectedColor = pinInfo.unlocked ? new Color(0f, 0.75f, 0f) : new Color(0.75f, 0f, 0f);
        cb.highlightedColor = pinInfo.unlocked ? Color.green : new Color(0.20f, 0f, 0f);
        cb.pressedColor = pinInfo.unlocked ? new Color(0f, 0.90f, 0f) : Color.black;
        btn.colors = cb;
    }
}

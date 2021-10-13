using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Simon_CustomButton_ : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Variables
    public Color pressedColor, hoverColor;

    public Sprite btnUp, btnDown;

    public UnityEvent buttonEvent;

    public bool holdButton;

    private Color startingColor;

    private bool buttonPressed, buttonDown;

    private Simon_FollowTarget_ player;
    private Image image;
    #endregion

    private void Awake()
    {
        if(buttonEvent == null) { buttonEvent = new UnityEvent(); }
    }
    private void Start()
    {
        player = FindObjectOfType<Simon_FollowTarget_>();
        image = GetComponent<Image>();
        startingColor = image.color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
        buttonPressed = true;
        image.sprite = btnDown;
        image.color = pressedColor;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        buttonPressed = false;
        if (!holdButton) { buttonEvent.Invoke(); }
        image.sprite = btnUp;
        image.color = startingColor;
    }
    public void OnPointerEnter(PointerEventData data)
    {
        player.overButton = true;
        if (buttonPressed) return;
        image.color = hoverColor;
    }
    public void OnPointerExit(PointerEventData data)
    {
        player.overButton = false;
        image.color = startingColor;
    }
    private void Update()
    {
        if (!holdButton || !buttonDown) return;
        buttonEvent.Invoke();
        image.color = pressedColor;
    }
}
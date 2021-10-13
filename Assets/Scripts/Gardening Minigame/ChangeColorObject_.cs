using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ChangeColorObject_ : GardenClickableObject_
{

    [SerializeField]public bool isPlanter;
    [FMODUnity.EventRef] private string paintSound = "event:/Global/Effect/Paint_Brush";
   
    bool changed;
    bool clickedThis;
    int colorIndex;
   
    protected override void Start()
    {
        base.Start();
       
    }
    public int GetColorIndex()
    {
        return colorIndex;
    }
    private void ApplyColor()
    {
        if (gardeningGameController.HasPickedColor() && isThisObjectMode)
        {
            playerAnimator.SetTrigger("placing");
            FMODUnity.RuntimeManager.PlayOneShot(paintSound, transform.position);
            spriteRenderer.color = gardeningGameController.ChosenColor();
            colorIndex = gardeningGameController.GetColorIndex();
            gardeningGameController.UpdateData();

            gsm.SetInt($"gardening_{gameObject.name}_color", gardeningGameController.GetColorIndex());
        }
    }

    private void FixedUpdate()
    {
        if (GetComponent<GardenMovePlayer_>().IsAtTarget()&&!changed && clickedThis)
        {
            ApplyColor();
            changed = true;
            clickedThis = false;
        }
    }
    public override void UpdateObject()
    {
       
        colorIndex = gsm.GetInt($"gardening_{gameObject.name}_color");
        spriteRenderer.color = gardeningGameController.colorsAvailable[colorIndex];
       
        isThisObjectMode = (gardeningGameController.GetMode() == GardeningGameController_.CustomizationMode.Color);

        if (!isBoth) collider.enabled = isThisObjectMode;

    }
        



    protected override void OnMouseOver()
    {
        if (isThisObjectMode)
        {
            uiSpriteRenderer.color = onHover;
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        changed = false;
        clickedThis = true;
    }

}




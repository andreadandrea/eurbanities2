using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class PlantObject_ : GardenClickableObject_
{
    [SerializeField] GameObject arrow;
    [SerializeField] SpriteRenderer plantSpriteRenderer;
    bool changed;
    int plantSpriteIndex;
    [FMODUnity.EventRef] [SerializeField] private string digSound = "event:/Gardening/Shovel";

    public int GetPlantIndex()
    {
        return plantSpriteIndex;
    }
    public void SetPlantIndex(int value)
    {
        plantSpriteIndex = value;
    }
    private void ApplyPlant()
    {
        if (gardeningGameController.HasPickedSeed() && isThisObjectMode)
        {
            playerAnimator.SetTrigger("placing");
            FMODUnity.RuntimeManager.PlayOneShot(digSound, transform.position);
            plantSpriteRenderer.sprite = gardeningGameController.ChosenPlant();
            plantSpriteIndex = gardeningGameController.GetPlantIndex();
            gardeningGameController.UpdateData();
            gsm.SetInt($"gardening_{gameObject.name}_plant", gardeningGameController.GetPlantIndex());
        }
    }

    
   
    protected override void Start()
    {
        base.Start();
        
       
    }
    private void FixedUpdate()
    {
        if (GetComponent<GardenMovePlayer_>().IsAtTarget() && !changed)
        {

            ApplyPlant();
            changed = true;
        }
    }
    public override void UpdateObject()
    {

        plantSpriteIndex = gsm.GetInt($"gardening_{gameObject.name}_plant");
        plantSpriteRenderer.sprite = gardeningGameController.plantSprites[plantSpriteIndex];
      
        isThisObjectMode = (gardeningGameController.GetMode() == GardeningGameController_.CustomizationMode.Plant);
        arrow.SetActive(isThisObjectMode);
        if (!isBoth) collider.enabled = isThisObjectMode;

        if (isThisObjectMode || isBoth) uiSpriteRenderer.sortingOrder = 2;
        else uiSpriteRenderer.sortingOrder = 1;

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
    }


}












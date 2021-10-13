using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GardenClickableObject_ : MonoBehaviour
{
    protected Collider2D collider;
    [SerializeField] protected bool isBoth;
    protected bool isThisObjectMode;
    [SerializeField] protected Color onHover, normal, clicked;
    [SerializeField] public SpriteRenderer uiSpriteRenderer;
    public SpriteRenderer spriteRenderer;
    protected GardeningGameController_ gardeningGameController;
    protected GlobalStateManager_ gsm;
    [SerializeField]protected Animator playerAnimator;
    protected virtual void Awake()
    {
        gsm = GlobalStateManager_.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        normal = uiSpriteRenderer.color;
        gardeningGameController = FindObjectOfType<GardeningGameController_>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
       // uiSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        playerAnimator = FindObjectOfType<Simon_FollowTarget_>().gameObject.GetComponentInChildren<Animator>();
    }
    public abstract void UpdateObject();

    protected abstract void OnMouseOver();

    protected virtual void OnMouseExit()
    {
        uiSpriteRenderer.color = normal;
    }

    protected virtual void OnMouseDown()
    {
        // uiSpriteRenderer.color = clicked;
        if (gardeningGameController.StandingStill() && gardeningGameController.GetMode() != GardeningGameController_.CustomizationMode.None)
        {
            gardeningGameController.SetPosToGo(GetComponent<GardenMovePlayer_>().positionToGo.position);
            GetComponent<GardenMovePlayer_>().SetGoToTarget(true);
        }


    }
}

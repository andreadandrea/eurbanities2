using System;
using UnityEngine;
using Cinemachine;

public class Simon_FollowTarget_ : MonoBehaviour
{
    #region Variables
    [Header("Movement Speed")]

    [Range(0f, 10f)][SerializeField] private float walkSpeed = 3f;

    [Range(0f, 10f)][SerializeField] private float runSpeed = 5f;

    [Tooltip("The maximum speed the player can reach when swipe movement is enabled")]
    [Range(0f, 10f)][SerializeField] private float maxSwipeSpeed = 7f;

    [Header("Object references")]

    [Tooltip("The object named Buttons Canvas")]
    [SerializeField] private GameObject buttonsCanvas;

    [Tooltip("The object named dialog camera target on the dialog canvas object")]
    [SerializeField] private GameObject dialogCameraTarget;

    [Header("General Settings")]

    [Tooltip("The distance between the player and the character they are talking to")]
    [Range(0f, 15f)][SerializeField] private float distanceToDialogTarget = 2f;

    [Tooltip("The minimum distance from the character the player can click")]
    [Range(0f, 10f)][SerializeField] private float clickDeadZone = 1.5f;

    [Tooltip("Which talking animation the player should have")]
    [Range(1, 4)][SerializeField] int talkingAnimation = 3;

    [Tooltip("Flip the direction the player moves when using swipe movement")]
    [SerializeField] bool flipSwipeDirection;

    [Tooltip("Set this to true if the player is inside")]
    public bool insideArea = false;

    private float moveSpeed;

    private Vector2 targetPos;
    private Vector2 buttonTargetPos;
    private Vector2 mousePos;

    private Vector2 startSwipe = Vector2.zero;
    private Vector2 endSwipe = Vector2.zero;

    [NonSerialized] public bool overButton;
    [NonSerialized] public bool reachedDialogTarget;

    private Vector3 startLocalScale;
    private Vector3 lastPosition;

    private float swipeDir = 0, moveDir = 1;

    private bool moveToDialog = false, moveToDoor = false, swipeMovePlayer = false, mouseHeldDown = false;

    private bool dialogCamera = false;

    bool checkSpeed = true;

    Dialog currentDialog;

    Rigidbody2D rigidBody2D;

    public static event Action<Dialog> StartDialog;
    public static event Action DialogCamera;
    public static event Action MainCamera;

    private Animator animator;
    private GameObject dialogTarget;
    GlobalStateManager_ GSM;
    #endregion
    private void Awake()
    {
        Simon_SwitchCMCamera_.OnCameraBlendFinished += OpenDialogMenu;
        Simon_OpenPhone_.PhoneOpen += OpenPhone;
        Simon_NPC_.DialogStart += SetDialogTarget;
        PlayerResponse_.CloseDialog += CloseDialogMenu;
        Simon_DialogManager_.MainCameraActive += MainCameraActive;
        Simon_DialogManager_.DialogCameraActive += DialogCameraActive;
        Simon_Door_.DoorClicked += MoveToDoor;
    }
    private void OnDestroy()
    {
        Simon_SwitchCMCamera_.OnCameraBlendFinished -= OpenDialogMenu;
        Simon_OpenPhone_.PhoneOpen -= OpenPhone;
        Simon_NPC_.DialogStart -= SetDialogTarget;
        PlayerResponse_.CloseDialog -= CloseDialogMenu;
        Simon_DialogManager_.MainCameraActive -= MainCameraActive;
        Simon_DialogManager_.DialogCameraActive -= DialogCameraActive;
        Simon_Door_.DoorClicked -= MoveToDoor;
    }
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        GSM = GlobalStateManager_.Instance;
        rigidBody2D = GetComponent<Rigidbody2D>();

        targetPos = transform.position;
        lastPosition = transform.position;

        startLocalScale = transform.localScale;
        moveSpeed = walkSpeed;
        swipeMovePlayer = false;
    }

    private void Update()
    {
        if (GSM.GetBool("buttons") && GSM.GetBool("canMove"))
        {
            if (buttonsCanvas != null)
                buttonsCanvas.SetActive(true);
        }
        else
        {
            if (buttonsCanvas != null)
                buttonsCanvas.SetActive(false);
        }

        if ( !GSM.GetBool("canMove")) { moveSpeed = 0; }
        else
        {
            if ( GSM.GetBool("pointAndClick") && GSM.GetBool("canMove") && !overButton)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mouseHeldDown = true;
                }
                if(Input.GetMouseButtonUp(0))
                {
                    mouseHeldDown = false;
                    checkSpeed = true;
                }
                if(mouseHeldDown)
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Mathf.Abs(transform.position.x - mousePos.x) > clickDeadZone)
                    {
                        targetPos = new Vector2(mousePos.x, transform.position.y);
                        moveDir = Mathf.Sign(targetPos.x - transform.position.x);
                    }
                    SpriteDirection(moveDir);
                    if(checkSpeed)
                    {
                        if (Mathf.Abs(transform.position.x - mousePos.x) < 5f)
                        {
                            moveSpeed = walkSpeed;
                        }
                        else
                        {
                            moveSpeed = runSpeed;
                        }
                        checkSpeed = false;
                    }

                }
            }
            else if(GSM.GetBool("swipe") && GSM.GetBool("canMove") && !overButton)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 targetPos = new Vector2(mousePos.x, transform.position.y);

                    startSwipe = targetPos;
                }
                if (Input.GetMouseButtonUp(0) && !overButton) {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 targetPos = new Vector2(mousePos.x, transform.position.y);

                    endSwipe = targetPos;
                    Invoke("StopPlayer", 1f);
                    if (flipSwipeDirection) { swipeDir = endSwipe.x - startSwipe.x; }
                    else { swipeDir = startSwipe.x - endSwipe.x; }
                    moveSpeed = Mathf.Abs(swipeDir);
                    moveDir = Mathf.Sign(swipeDir);
                    SpriteDirection(moveDir);
                    swipeMovePlayer = true;
                }
            }
        }
    }
    private void SetDialogTarget(GameObject dialogTarget, Dialog currentDialog)
    {
        GSM.SetBool("inDialog", true);
        mouseHeldDown = false;
        GSM.SetBool("canMove", false);
        this.dialogTarget = dialogTarget;
        this.currentDialog = currentDialog;
        if (insideArea) { targetPos = transform.position; }
        else { targetPos = new Vector2(dialogTarget.transform.position.x + distanceToDialogTarget, transform.position.y); }
        SpriteDirection(Mathf.Sign(targetPos.x - transform.position.x));
        moveToDialog = true;
    }
    private void StopPlayer()
    {
        swipeMovePlayer = false;
        if(!IsInvoking("StopPlayer")) rigidBody2D.velocity = Vector2.zero;
    }
    private void FixedUpdate()
    {
        if(!GSM.GetBool("canMove")) { rigidBody2D.velocity = Vector2.zero; }
        else if(swipeMovePlayer) {
            if (Mathf.Abs(swipeDir) < 0.5f) swipeDir = 0f;
            if (Mathf.Abs(swipeDir) > maxSwipeSpeed) swipeDir = maxSwipeSpeed * moveDir;
            rigidBody2D.velocity = new Vector2(swipeDir, 0);
        }

        if (GSM.GetBool("pointAndClick") || moveToDialog || moveToDoor)
        {
            if (moveToDialog) { transform.position = Vector3.MoveTowards(transform.position, targetPos, runSpeed * Time.deltaTime); }
            if ( GSM.GetBool("canMove") || moveToDoor && GSM.GetBool("canMove")) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
            if (transform.position.x == targetPos.x && moveToDialog)
            {
                moveToDialog = false;
                SpriteDirection(-1);
                dialogCameraTarget.transform.position = Vector3.Lerp(transform.position, dialogTarget.transform.position, 0.5f);
                DialogCamera?.Invoke();
            }
        }

        animator.SetBool("walking", transform.position != lastPosition && moveSpeed < runSpeed);
        animator.SetBool("running", transform.position != lastPosition && moveSpeed >= runSpeed);
        lastPosition = transform.position;
    }
    public void ButtonLeft()
    {
        ButtonMove(-1);
    }
    public void ButtonRight()
    {
        ButtonMove(1);
    }
    private void ButtonMove(float dir)
    {
        if (!GSM.GetBool("canMove") || !overButton) return;
        moveSpeed = runSpeed;
        buttonTargetPos = new Vector3(transform.position.x + dir, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, buttonTargetPos, moveSpeed * Time.deltaTime);
        targetPos = transform.position;
        SpriteDirection(dir);
    }
    private void SpriteDirection(float dir)
    {
        transform.localScale = new Vector2(dir * startLocalScale.x, transform.localScale.y);
    }
    private void OpenPhone()
    {
        mouseHeldDown = false;
        targetPos = transform.position;
    }
    void OpenDialogMenu()
    {
        if (!dialogCamera) return;
        StartDialog?.Invoke(currentDialog);
        dialogTarget.GetComponent<Simon_NPC_>().StartDialog();
        switch(talkingAnimation)
        {
            case 1:
                animator.SetBool("talking", true);
                break;
            case 2:
                animator.SetBool("talking2", true);
                break;
            case 3:
                animator.SetBool("talking3", true);
                break;
            case 4:
                animator.SetBool("talking4", true);
                break;
        }
    }
    void CloseDialogMenu()
    {
        GSM.SetBool("inDialog", false);
        GSM.SetBool("canMove", true);
        MainCamera?.Invoke();
        dialogTarget.GetComponent<Simon_NPC_>().EndDialog();
        animator.SetBool("talking", false);
        animator.SetBool("talking2", false);
        animator.SetBool("talking3", false);
        animator.SetBool("talking4", false);
    }
    void MoveToDoor(GameObject door) {
        moveToDoor = true;
        targetPos.x = door.transform.position.x;
    }
    void MainCameraActive()
    {
        dialogCamera = false;
    }
    void DialogCameraActive()
    {
        dialogCamera = true;
    }
}
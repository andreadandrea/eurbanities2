using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Simon_NPC_ : MonoBehaviour
{
    public static event Action<GameObject, Dialog> DialogStart;
    public static event Action<GameObject, MonoLog> MonologStart;

    [SerializeField] Dialog dialog;
    [SerializeField] Dialog forcedDialog;
    [SerializeField] string boolToCheck;
    [SerializeField] bool forceDialog;
    private bool sWiTcH = false;
    [SerializeField] MonoLog monolog;
    [SerializeField] private string[] GSMBoolToSet;
    [Range(1, 4)] [SerializeField] int talkingAnimation = 2;
    public UnityEvent startedTalking;

    private GlobalStateManager_ GSM;
    private Simon_FollowTarget_ player;
    Animator animator;
    private void Start()
    {
        GSM = FindObjectOfType<GlobalStateManager_>();
        player = FindObjectOfType<Simon_FollowTarget_>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("idleSpeedMultiplier", UnityEngine.Random.Range(0.8f, 1.3f));
        animator.SetFloat("idleCycleOffset", UnityEngine.Random.Range(0.8f, 1.3f));
}

    private void OnMouseDown()
    {
        if (GSM.GetBool("inDialog")) return;
        startedTalking.Invoke();
        SetGSMBool();
        if (!sWiTcH && monolog != null && dialog == null) 
        {
            RotateTowardsPlayer();
            monolog.Initiate();
            SetSwitch();
        }
        if (sWiTcH)
        {
            RotateTowardsPlayer();
            Debug.Log("grait mait, rait eight out of eight");
            MonologStart?.Invoke(gameObject, monolog);
            return;
        }
        else
        DialogStart?.Invoke(gameObject, dialog);
        if (!sWiTcH && monolog != null) StartCoroutine(ConnectToSystem());
    }
    IEnumerator ConnectToSystem()
    {
        yield return new WaitUntil(() => PlayerResponse_.PR != null);
        PlayerResponse_.PR.StartInitiation += monolog.Initiate;
        PlayerResponse_.PR.StartInitiation += SetSwitch;
    }
    private void SetSwitch()
    {
        sWiTcH = true;
    }
    private void Update()
    {
        if (!forcedDialog) return;

        if(GSM.GetBool(boolToCheck))
        {
            DialogStart?.Invoke(gameObject, forcedDialog);
        }
    }

    // Sets selected GSM bool to true
    private void SetGSMBool()
    {
        foreach (var str in GSMBoolToSet)
        {
            GSM.SetBool(str, true);
        }
    }

    public void StartDialog()
    {
        switch (talkingAnimation)
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
        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        float yRotation = 90 * (Mathf.Sign(player.transform.position.x - transform.position.x) - 1);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void EndDialog()
    {
        animator.SetBool("talking", false);
        animator.SetBool("talking2", false);
        animator.SetBool("talking3", false);
        animator.SetBool("talking4", false);
    }
} 

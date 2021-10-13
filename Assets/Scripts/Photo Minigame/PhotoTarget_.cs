using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class PhotoTarget_ : MonoBehaviour
{
    PhoneCamera_ phoneCamera_ => FindObjectOfType<PhoneCamera_>();

    public string accessibilityName;
    public UnityEvent accessibilityFixed;
    [EventRef] [SerializeField] private string stickerSound = "event:/Global/Sticker_Get";

    private GlobalStateManager_ GSM => GlobalStateManager_.Instance;

    // Start is called before the first frame update
    void Start()
    {
        IfFound();
    }


    public void SendPhoto(Animator animator)
    {
        if (GSM.GetBool(accessibilityName) || !phoneCamera_.PhotoTargetInFrame()) return;

        RuntimeManager.PlayOneShot(stickerSound, transform.position);
        animator.SetTrigger("SendPhoto");
        GSM.SetBool(accessibilityName, true);
        Debug.Log("GOTCHA!");
    }

    private void IfFound()
    {
        if(GSM.GetBool(accessibilityName))
            accessibilityFixed.Invoke();
    }
}

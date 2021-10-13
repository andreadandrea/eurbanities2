using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSound_ : MonoBehaviour
{
    [FMODUnity.EventRef][SerializeField]
    private string trashed;
    [FMODUnity.EventRef][SerializeField]
    private string missed = "event:/Recycling/Feedback/Wrong_Bin";
    [FMODUnity.EventRef]
    [SerializeField]
    private string correct= "event:/Recycling/Feedback/Correct_Bin";
    [FMODUnity.EventRef]
    [SerializeField]
    private string hitGround = "event:/Recycling/Trash/Trash_Missing_Bin";

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "PlasticTrashcan" || collision.tag == "MetalTrashcan" || collision.tag == "GlassTrashcan")
        {
            if (this.gameObject.tag == "Plastic")
                FMODUnity.RuntimeManager.PlayOneShot(trashed, transform.position);

            if (this.gameObject.tag == "Metal")
                FMODUnity.RuntimeManager.PlayOneShot(trashed, transform.position);

            if (this.gameObject.tag == "Glass")
                FMODUnity.RuntimeManager.PlayOneShot(trashed, transform.position);
        }
        if (collision.tag == "Ground")
        {
            if (this.gameObject.tag == "Plastic")
            {
                FMODUnity.RuntimeManager.PlayOneShot(trashed, transform.position);
                FMODUnity.RuntimeManager.PlayOneShot(hitGround, transform.position);
            }

            if (this.gameObject.tag == "Metal")
            {
                FMODUnity.RuntimeManager.PlayOneShot(trashed, transform.position);
                FMODUnity.RuntimeManager.PlayOneShot(hitGround, transform.position);
            }
            
            if (this.gameObject.tag == "Glass")
            {
                FMODUnity.RuntimeManager.PlayOneShot(trashed, transform.position);
                FMODUnity.RuntimeManager.PlayOneShot(hitGround, transform.position);
            }     
        }
    }

    public void TrashMissedSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(missed, transform.position);
    }

    public void TrashCorrectSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(correct, transform.position);
    }
}

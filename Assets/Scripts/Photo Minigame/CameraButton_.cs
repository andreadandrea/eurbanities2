using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraButton_ : MonoBehaviour
{
    [SerializeField] private Animator flashAnimator;
    [SerializeField] [FMODUnity.EventRef] private string cameraSound;
    private Animator sendPhotoAnimator;
    PhoneCamera_ phoneCamera_;
    

    Vector3 offset;
    Vector3 startPos;
    // Start is called before the first frame update

    void Start()
    {
        

        sendPhotoAnimator = GameObject.Find("prefab_sendingPhotoUI").GetComponent<Animator>();
        phoneCamera_ = FindObjectOfType<PhoneCamera_>();
        offset = phoneCamera_.transform.position - transform.position;
    }



    private void OnMouseDown()
    {
        Debug.Log("OnPointerDown");
        FMODUnity.RuntimeManager.PlayOneShot(cameraSound, transform.position);
        flashAnimator.SetTrigger("PhotoFlash");
        if (phoneCamera_.GetPhotoTarget() != null)
        {

            phoneCamera_.GetPhotoTarget().SendPhoto(sendPhotoAnimator);
        }
    }



    // Update is called once per frame
    void Update()
    {
        transform.position = phoneCamera_.transform.position - offset;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeController_ : MonoBehaviour
{
    [SerializeField]GameObject phoneOutline, cameraButton;
    Animator phoneAnimator;
    public bool cameraModeActive = false;
    [SerializeField] float movementSpeed = 2;
    Rigidbody2D rb;
    [Range(0,1)][SerializeField] float minDistanceToTransition = 0.3f;
    Transform transformTarget;
    Vector3 positionTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        phoneAnimator = GameObject.FindObjectOfType<Simon_OpenPhone_>().GetComponent<Animator>();
        transformTarget = Camera.main.transform;
        if(transformTarget == null) positionTarget = new Vector3(0, 0, 0);
     
        /*phoneOutline = GameObject.Find("PhoneOutline");
        cameraButton = GameObject.Find("PhotoButton");*/
    }

   

    private void FixedUpdate()
    {
        if (transformTarget != null) positionTarget = new Vector3(transformTarget.position.x,transformTarget.position.y,0); 

        float phoneDistance = Vector3.Distance(phoneOutline.transform.position, positionTarget);
        if (phoneDistance<= minDistanceToTransition) phoneAnimator.SetBool("InPosition", true);
        else phoneAnimator.SetBool("InPosition", false);

        if (phoneAnimator.GetCurrentAnimatorStateInfo(0).IsName("CameraMode") || (phoneAnimator.GetCurrentAnimatorStateInfo(0).IsName("MoveToPosition")))
        {
            if ((phoneAnimator.GetCurrentAnimatorStateInfo(0).IsName("MoveToPosition"))) MoveTowardsTarget();
            cameraModeActive = true;
        }
        else cameraModeActive = false;

        if (!cameraModeActive) phoneOutline.transform.position = positionTarget;
        phoneOutline.SetActive(cameraModeActive);
        cameraButton.SetActive(cameraModeActive);
       
    }


    
    private void MoveTowardsTarget()
    {
        Vector3 pos = Vector3.Lerp(phoneOutline.transform.position, positionTarget, movementSpeed * Time.deltaTime);
        phoneOutline.GetComponent<Rigidbody2D>().MovePosition(pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneCamera_ : MonoBehaviour
{
    bool photoTargetInFrame;

    [FMODUnity.EventRef] [SerializeField] private string targetSound = "event:/Global/Effect/Found_Accessibility_Issue";
    Rigidbody2D rb;
    Vector3 mousePos;
    PhotoTarget_ photoTarget_;
    Vector3 mouseOffset;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool PhotoTargetInFrame()
    {
        return photoTargetInFrame;
    }

    private void OnMouseDown()
    {
        mouseOffset = transform.position - mousePos;
    }
    private void OnMouseDrag()
    {
        rb.MovePosition(mousePos + mouseOffset);
    }

    public PhotoTarget_ GetPhotoTarget()
    {
        return photoTarget_;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PhotoTarget")
        {
            photoTargetInFrame = true;
            FMODUnity.RuntimeManager.PlayOneShot(targetSound, transform.position);
            if (collision.GetComponent<PhotoTarget_>() != null)
            {
                Debug.Log("TargetChangd");
                photoTarget_ = collision.GetComponent<PhotoTarget_>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PhotoTarget")
        {
            photoTargetInFrame = false;
        }
    }









}

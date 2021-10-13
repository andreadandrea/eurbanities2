using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysicalFunctionScript_ : MonoBehaviour
{
    bool eachOtherUpdate = false;
    #region variables
    public bool stopDancing = false;
    bool direction = true, changeingDirection = false;

    float wiggleDirection = 1;
    public float shakeRange, WiggleSpeed;

    [Range(1, 10)]public float wiggleVelocity = 10;
    [Range(5, 20)]public float wiggleRange = 10;

    Vector2 startingPos;

    //public FunnyTextHandler.VisualApperance visualApperance;
    public FunnyTextHandler_.PhysicalApperance physicalApperance;
    #endregion
    private void Start()
    {
        UpdatePos();
        shakeRange *= transform.parent.parent.localScale.x;
    }
    public void UpdatePos()
    {
        startingPos = transform.position;
    }
    private void FixedUpdate()
    {
        if (stopDancing)
        {
            transform.position = startingPos;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            return;
        }
        eachOtherUpdate = !eachOtherUpdate;
        if (eachOtherUpdate) return;
        switch (physicalApperance)
        {
            case FunnyTextHandler_.PhysicalApperance.shake:
                {
                    transform.position = startingPos + new Vector2(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange));
                    return;
                }
            case FunnyTextHandler_.PhysicalApperance.wiggle:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + wiggleDirection * Time.deltaTime * WiggleSpeed));
                    if (!changeingDirection)
                        if (transform.rotation.eulerAngles.z < 360 - wiggleRange && !direction && transform.rotation.eulerAngles.z > 100 || 
                            transform.rotation.eulerAngles.z > wiggleRange && direction && transform.rotation.eulerAngles.z < 100)
                            StartCoroutine(SwitchPolarity());
                    return;
                }
        }
    }
    IEnumerator SwitchPolarity()
    {
        changeingDirection = true;
        if (!direction)
            while (wiggleDirection < 1)
            {
                wiggleDirection += Time.deltaTime * wiggleVelocity;
                yield return new WaitForFixedUpdate();
            }
        else
            while (wiggleDirection > -1)
            {
                wiggleDirection -= Time.deltaTime * wiggleVelocity;
                yield return new WaitForFixedUpdate();
            }
        changeingDirection = false;
        direction = !direction;
    }
}

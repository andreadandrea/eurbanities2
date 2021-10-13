using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PinWalker_ : MonoBehaviour
{
    [SerializeField] private Button[] exitButtons;
    public Transform pin;
    public Transform currentSubPin;
    public float movementSpeed;
    public bool walking;

    private Queue<Transform> finalPath;
    private PinInteraction_ PinInteractionHandler;
    private float currentMovementSpeed;
    private float localScaleX;
    private float distanceCheck;

    private Animator anim;

    [SerializeField] private Transform goals;

    private void Start()
    {
        localScaleX = transform.localScale.x;
        anim = GetComponentInChildren<Animator>();
        PinInteractionHandler = FindObjectOfType<PinInteraction_>();
    }

    private void OnEnable()
    {
        Transform finalTransform = null;
        
        
        foreach (Transform goalTransform in goals)
        {
            var goal = goalTransform.GetComponent<PinInfo_>();
            if (goal.scene != SceneManager.GetSceneByBuildIndex(GlobalStateManager_.Instance.GetInt("currentScene")).name) continue;
            
            finalTransform = goalTransform;
            break;
        }

        if (ReferenceEquals(finalTransform, null)) return;

        transform.position = finalTransform.position;
    }

    private void FixedUpdate()
    {

        if (!walking || ReferenceEquals(currentSubPin, null) || ReferenceEquals(finalPath, null)) return;
        
        distanceCheck = currentMovementSpeed / 10;
        
        if (Vector2.Distance(transform.position, currentSubPin.position) < distanceCheck)
        {
            if (finalPath.Count == 0)
            {
                finalPath = null;
                walking = false;
                currentSubPin = null;
                transform.position = pin.position;
            }
            else
            {
                /*
                PinInfo info = currentSubPin.GetComponent<PinInfo>();
                if(!info.isGoal)
                    movementSpeed = info.movementspeed;
                */
                
                currentSubPin = finalPath.Dequeue();
            }

            foreach (var button in exitButtons)
            {
                button.interactable = !walking;
            }
            
            anim.SetBool("walking", walking);
        }
        else
        {
            var trans = transform;
            var position = trans.position;
            var velocity = currentSubPin.position - position;
            var move = velocity.normalized / 10 * currentMovementSpeed;
            
            position += move;
            trans.position = position;

            bool flip = velocity.x > 0;
           
            var localScale = trans.localScale;
            localScale = new Vector3(localScaleX * (flip ? 1 : -1), localScale.y, localScale.z);
            trans.localScale = localScale;
        }
    }

    public IEnumerator SetPath(Queue<Transform> path)
    {
        finalPath = path;
        currentMovementSpeed = PinInteractionHandler.checkdist / 5 * movementSpeed;
        walking = true;
        yield break;
    }
}

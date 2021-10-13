using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenMovePlayer_ : MonoBehaviour
{
    [SerializeField] public Transform positionToGo;
    GameObject player;
   
    //Transform pos;
    bool isBackAtStartPos;
    bool goToTarget;
    GardeningGameController_ gardenGameController;
    Simon_GameSession_ gameSession;
    public bool isAtTarget;
    bool otherMove = false;

   // Vector3 playerStartPos;
    // Start is called before the first frame update
    void Start()
    {
        gardenGameController = FindObjectOfType<GardeningGameController_>();
        gameSession = FindObjectOfType<Simon_GameSession_>();
        if (positionToGo == null) positionToGo = this.transform;
        if (player == null) player = FindObjectOfType<Simon_FollowTarget_>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        isAtTarget = IsAtTarget();
        GoToPos();
    }


    private void GoToPos()
    {
        isBackAtStartPos = player.transform.position == gardenGameController.goBackPos.position;
        if (goToTarget)
        {
            otherMove = true;


            Vector3 move = Vector3.MoveTowards(player.transform.position, positionToGo.position, gardenGameController.otherMoveSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(move);
            if (IsAtTarget()) goToTarget = false;
        }
    }

    public void SetGoToTarget(bool value)
    {
        goToTarget = value;
        //if (goToTarget &&!otherMove) playerStartPos = player.transform.position;
    }

    public bool IsAtTarget()
    {
        return player.transform.position == positionToGo.position;
    }
}

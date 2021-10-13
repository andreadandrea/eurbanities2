using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
public class YogaGameController : MonoBehaviour
{
    public float stateTransitionTime = 2;

    public float value = 1f, scaleSpeed; //1 by default in inspector
    private float currentScaleSpeed;
    [Range(0f, 1f)] public float breatheTimeSlow;
    public float[] areaSizes;
    public Transform[] breatheInSizes, breatheOutSizes;
    private Vector3[] breatheInStartSizes, breatheOutStartSizes;
    [SerializeField] float outSizeMultiplierRound2, outSizeMultiplierRound3, inSizeMultiplierRound2, inSizeMultiplierRound3;
    float startUpscalePoint;
    public StateMachine state = StateMachine.Start;
    int breatheType;
    public GameObject breatheCircle, breatheInPointBoard, breatheOutPointBoard;
    public bool upScale = false, isBreatheInMode = true, isBreatheOutMode;

	[SerializeField][EventRef] private string breatheIn;
	[SerializeField][EventRef] private string breatheOut;
	[SerializeField][EventRef] private string click;
	
    bool hasClicked = false;

    public GameObject balanceThing, balanceModeObjects;
    private float balanceThingCurrentSpeed;
    public float balanceThingSpeedNormal;
    [Range(0f, 1f)] public float balanceTimeSlow;
    public float xOffsetBalanceThing = 3;
    private bool isBalanceMode = false;
    public float[] balanceBarAreas;

    public int round = 1;
    
    
    public int score, currentScore, maximumScore;
    public Text scoreText;
    public GameObject scoreScreen,startScreen;
    public Text scoreTextScreen;
    public Text roundText;

    bool isPaused;
    private void Start()
    {
        breatheOutStartSizes = new Vector3[breatheOutSizes.Length];
        breatheInStartSizes = new Vector3[breatheInSizes.Length];
        for (int i = 0; i < breatheOutSizes.Length; i++)
        {
            breatheOutStartSizes[i] = breatheOutSizes[i].localScale;
        }
        for (int i = 0; i < breatheInSizes.Length; i++)
        {
            breatheInStartSizes[i] = breatheInSizes[i].localScale;
        }
        balanceThingCurrentSpeed = balanceThingSpeedNormal;
        currentScaleSpeed = scaleSpeed;
    }
    public enum StateMachine
    {
       Start, BreatheIn, BreatheOut, Balance, Score
    }
    //This method is executed every frame
    private void Update()
    {
        roundText.text = "Round: " + round.ToString();
        scoreText.text = currentScore.ToString();
        scoreTextScreen.text = currentScore.ToString() + "/" + maximumScore.ToString();


        startScreen.SetActive(state == StateMachine.Start);
        scoreScreen.SetActive(state == StateMachine.Score);
        balanceModeObjects.SetActive(state == StateMachine.Balance);
        breatheInPointBoard.SetActive(state == StateMachine.BreatheIn);
        breatheOutPointBoard.SetActive(state == StateMachine.BreatheOut);
        breatheCircle.SetActive(state == StateMachine.BreatheOut || state == StateMachine.BreatheIn);
        switch (state)
        {
            case StateMachine.Start:

                break;
            case StateMachine.BreatheIn:

                if (round >= 4)
                {
                   /* isBalanceMode = false;
                    isBreatheInMode = false;
                    isBreatheOutMode = false;*/
                    state = StateMachine.Score;
                }

                breatheType = 0;
                BreathMode();
                break;

            case StateMachine.Balance:

                BalanceMode();
                break;
            case StateMachine.BreatheOut:

                breatheType = 1;
                BreathMode();
                break;

            case StateMachine.Score:

                break;


        }




    }

    public void ChangeState(int value)
    {
        switch (value)
        {
            case 1:
                state = StateMachine.Start;
                break;
            case 2:
                state = StateMachine.BreatheIn;
                break;
            case 3:
                state = StateMachine.Balance;
                break;
            case 4:
                state = StateMachine.BreatheOut;
                break;
        }
    }

    IEnumerator WaitToChangeState()
    {
        Time.timeScale = 1;
        currentScaleSpeed = 0;
        balanceThingCurrentSpeed = 0;
        yield return new WaitForSeconds(stateTransitionTime);
        balanceThingCurrentSpeed = balanceThingSpeedNormal;
        currentScaleSpeed = scaleSpeed;

        ChangeState();
    }

    private void BalanceMode()
    {


        Vector3 movement = Vector3.right * Time.deltaTime * balanceThingCurrentSpeed;

        balanceThing.transform.Translate(movement);

        if (balanceThing.transform.position.x >= xOffsetBalanceThing)
        {
            balanceThingCurrentSpeed = balanceThingSpeedNormal * -1;
        }
        if (balanceThing.transform.position.x <= -xOffsetBalanceThing)
        {
            balanceThingCurrentSpeed = balanceThingSpeedNormal * 1;

        }

        CheckAreaBalanceMode();
        if (Input.GetMouseButtonDown(0) && !hasClicked)
        {
            hasClicked = true;
            currentScore += CheckAreaBalanceMode();
            StartCoroutine(WaitToChangeState());
			RuntimeManager.PlayOneShot(click, transform.position);

        }


    }


    private void BreathMode()
    {

        if (state == StateMachine.BreatheIn) startUpscalePoint = breatheInSizes[3].localScale.x;
        else if(state == StateMachine.BreatheOut) startUpscalePoint = breatheOutSizes[3].localScale.x;
        if (value >= breatheOutSizes[0].localScale.x)
        {
            upScale = false;
        }
        else if (value <= startUpscalePoint)
        {
            upScale = true;
        }


        if (upScale)
        {

            value += Time.deltaTime * currentScaleSpeed;
        }
        else
        {
            value += Time.deltaTime * -currentScaleSpeed;
        }
        //we store scale of this transform in temporary variable
        Vector3 tempScale = transform.localScale;

        //We change the values for this saved variable (not actual transform scale)
        tempScale.x = value;
        tempScale.y = value;
        tempScale.z = value;

        //We assign temp variable back to transform scale
        breatheCircle.transform.localScale = tempScale;





        CheckAreaBreatheMode();

        if (Input.GetMouseButtonDown(0) && !hasClicked)
        {
			if(state == StateMachine.BreatheIn)
			{
				RuntimeManager.PlayOneShot(breatheIn, transform.position);
			}
			else if (state == StateMachine.BreatheOut)
			{
				RuntimeManager.PlayOneShot(breatheOut, transform.position);
			}
			RuntimeManager.PlayOneShot(click, transform.position);
            hasClicked = true;
            currentScore += CheckAreaBreatheMode();

            currentScaleSpeed = 0;
            StartCoroutine(WaitToChangeState());


        }
    }

    private void ChangeDifficulty()
    {
        if (round == 1)
        {
            breatheInSizes[2].localScale = breatheInStartSizes[2];
            breatheOutSizes[1].localScale = breatheOutStartSizes[1];
        }
        else if (round == 2)
        {
            breatheInSizes[2].localScale = breatheInStartSizes[2] * inSizeMultiplierRound2;
            breatheOutSizes[1].localScale = breatheOutStartSizes[1] * outSizeMultiplierRound2;
        }
        else if (round == 3)
        {
            breatheInSizes[2].localScale = breatheInStartSizes[2] * inSizeMultiplierRound3;
            breatheOutSizes[1].localScale = breatheOutStartSizes[1] * outSizeMultiplierRound3;
        }
    }

    private void ChangeState()
    {
        hasClicked = false;
        //ramp difficulty by scaling the point areas

        
     

        if (state == StateMachine.BreatheIn || state == StateMachine.BreatheOut)
        {
           /* isBalanceMode = true;
            isBreatheInMode = false;
            isBreatheOutMode = false;*/
            state = StateMachine.Balance;
            int i = Random.Range(0, 2);
            if(i==0)balanceThing.transform.position = new Vector3(-xOffsetBalanceThing, balanceThing.transform.position.y);
            else if(i==1)balanceThing.transform.position = new Vector3(xOffsetBalanceThing, balanceThing.transform.position.y);
           
        }
        else if (state == StateMachine.Balance && breatheType ==1)
        {

            /*isBalanceMode = false;
            isBreatheInMode = true;
            isBreatheOutMode = false;*/

            upScale = false;
            value = breatheInSizes[0].transform.localScale.x;
            round++;

            state = StateMachine.BreatheIn;
        }
        else
        {

           /* isBalanceMode = false;
            isBreatheInMode = false;
            isBreatheOutMode = true;*/
            upScale = true;
            value = breatheOutSizes[3].transform.localScale.x;
            state = StateMachine.BreatheOut;
        }
        ChangeDifficulty();
    }
    private int CheckAreaBreatheMode()
    {
        switch (state)
        {
            case StateMachine.BreatheIn:

                if (value >= breatheInSizes[1].localScale.x)
                {
                    Time.timeScale = 1;
                    return 1;
                }
                else if(value<=breatheInSizes[1].localScale.x && value >= breatheInSizes[2].localScale.x)
                {
                    Time.timeScale = 1;
                    return 2;
                }
                else
                {
                    if (!hasClicked) Time.timeScale = breatheTimeSlow;
                    return 3;
                }
               
            case StateMachine.BreatheOut:
                if (value >= breatheOutSizes[1].localScale.x)
                {
                    if (!hasClicked) Time.timeScale = breatheTimeSlow;
                    
                    return 3;
                }
                else if (value <= breatheOutSizes[1].localScale.x && value >= breatheOutSizes[2].localScale.x)
                {
                    Time.timeScale = 1;
                    return 2;
                }
                else
                {
                    Time.timeScale = 1;
                    return 1;
                }

        }

        return 0;
    
    }
    private int CheckAreaBalanceMode()
    {
        if (balanceThing.transform.position.x <= balanceBarAreas[0] && balanceThing.transform.position.x >= -balanceBarAreas[0])
        {

            if (!hasClicked) Time.timeScale = balanceTimeSlow;
            // currentScore += 3;
            return 3;
        }
        else if (balanceThing.transform.position.x > balanceBarAreas[0] && balanceThing.transform.position.x <= balanceBarAreas[1] || balanceThing.transform.position.x < -balanceBarAreas[0] && balanceThing.transform.position.x >= -balanceBarAreas[1])
        {
            Time.timeScale = 1;
            //currentScore += 2;
            return 2;
        }
        else
        {
            Time.timeScale = 1;
            //  currentScore += 1;
            return 1;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(xOffsetBalanceThing, balanceThing.transform.position.y, balanceThing.transform.position.z), .2f);
        Gizmos.DrawSphere(new Vector3(-xOffsetBalanceThing, balanceThing.transform.position.y, balanceThing.transform.position.z), .2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(balanceBarAreas[0], balanceThing.transform.position.y, balanceThing.transform.position.z), 0.2f);
        Gizmos.DrawSphere(new Vector3(balanceBarAreas[1], balanceThing.transform.position.y, balanceThing.transform.position.z), 0.2f);
        Gizmos.DrawSphere(new Vector3(balanceBarAreas[2], balanceThing.transform.position.y, balanceThing.transform.position.z), 0.2f);
        Gizmos.DrawSphere(new Vector3(-balanceBarAreas[0], balanceThing.transform.position.y, balanceThing.transform.position.z), 0.2f);
        Gizmos.DrawSphere(new Vector3(-balanceBarAreas[1], balanceThing.transform.position.y, balanceThing.transform.position.z), 0.2f);
        Gizmos.DrawSphere(new Vector3(-balanceBarAreas[2], balanceThing.transform.position.y, balanceThing.transform.position.z), 0.2f);

        /*  Gizmos.color = Color.green;
          Gizmos.DrawSphere(new Vector3(0, 0, 0), areaSizes[1]*2);
          Gizmos.color = Color.yellow;
          Gizmos.DrawSphere(new Vector3(0, 0, 0), areaSizes[0] * 2);
        */

    }

    /*public void PauseGameToggle()
    {
        isPaused = !isPaused;

        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }*/
}

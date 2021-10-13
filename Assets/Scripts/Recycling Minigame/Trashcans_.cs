using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcans_ : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector3 mousePos;
    public Vector3 storedPos;
    private float currentMoveSpeed;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float metalPoints, glassPoints, plasticPoints;
    float totalPoints;
    float xDirection;
    [SerializeField] bool phoneControlls;
    

    bool rightButtonDown;
    bool rightButtonClicked;
    bool leftButtonDown;

    TrashSpawner_ trashSpawner;
    bool hasStarted = false;
    [SerializeField] float timeZeroToMax = 0.2f;
    float accelRateSpeed;
    Camera camera;
    private void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        accelRateSpeed = 1 / timeZeroToMax;
    }
    private void Start()
    {
        hasStarted = true;
        trashSpawner = FindObjectOfType<TrashSpawner_>();
        currentMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    PhotoTarget_ photoTarget_;
    Vector3 mouseOffset;




    private void Update()
    {

        if (!phoneControlls) xDirection = Input.GetAxis("Horizontal");

    }
    private void FixedUpdate()
    {
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        if (phoneControlls) Acceleration();

        rb.velocity = new Vector2(xDirection * Time.deltaTime * currentMoveSpeed, 0);
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mouseOffset = transform.position - mousePos;
    }
    private void OnMouseDrag()
    {
        rb.MovePosition(mousePos + mouseOffset);
    }
    public void ButtonInput(float direction)
    {

        if (direction > 0)
        {
            rightButtonDown = true;

            rightButtonClicked = true;
        }
        else
        {
            leftButtonDown = true;

            rightButtonClicked = false;

        }

    }

    public void ButtonUp()
    {
        rightButtonDown = false;
        leftButtonDown = false;
    }

    private void Acceleration()
    {

        if (rightButtonClicked)
        {

            if (rightButtonDown)
            {
                if (xDirection < 1)
                {

                    xDirection += accelRateSpeed * Time.deltaTime;
                }

            }
            else
            {
                if (xDirection > 0.01f)
                {
                    xDirection -= accelRateSpeed * Time.deltaTime;
                }
                else xDirection = 0;
            }
        }
        else
        {

            if (leftButtonDown)
            {
                if (xDirection > -1)
                {

                    xDirection -= accelRateSpeed * Time.deltaTime;
                }

            }
            else
            {
                if (xDirection < -0.01f)
                {
                    xDirection += accelRateSpeed * Time.deltaTime;
                }
                else xDirection = 0;
            }
        }


    }

    

   /*private void OnMouseDrag()
    {

        rb.MovePosition(new Vector3(mousePos.x, transform.position.y, transform.position.z));


    }*/
    public float GetTrashPoints(int trashType)
    {
        if (trashType == 0) return glassPoints;
        else if (trashType == 1) return metalPoints;
        else return plasticPoints;

    }

    public float GetTotalPoints()
    {
        return totalPoints;
    }
    public void AddPoints(float value, int trashType)
    {
        totalPoints++;
        if (trashType == 0) glassPoints += value;
        else if (trashType == 1) metalPoints += value;
        else plasticPoints += value;
    }
    public void SetPoint(float value, float totalValue)
    {
        glassPoints = value;
        metalPoints = value;
        plasticPoints = value;
        totalPoints = totalValue;
    }


    

}






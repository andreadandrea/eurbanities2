using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GardeningGameController_ : MonoBehaviour
{
    //The level of garden
    [SerializeField] GardenLevelState currentLevelState = GardenLevelState.Level1;



    //all gardening objects
    [SerializeField] PlantObject_[] allPlantObjects;
    [SerializeField] ChangeColorObject_[] allColorObjects;

    [SerializeField] int[] colorIndexArray;
    [SerializeField] int[] plantIndexArray;
    //planter versions
    ChangeColorObject_[] changeColorObjects_;
    [SerializeField] Sprite[] planterVersions;
    //unlockable colors, planks, plants
    [SerializeField] CanvasGroup[] plantLevelGroup;
    [SerializeField] CanvasGroup[] colorLevelGroup;
    [SerializeField] CanvasGroup[] plankLevelGroup;

    [SerializeField] GameObject[] gardenStateObject;

    [SerializeField] Sprite[] plankSprites;
    [SerializeField] SpriteRenderer plankSpriteRenderer;
    [SerializeField] SpriteRenderer plankUIFeedBackSpriteRenderer;
    GardenClickableObject_[] clickableObjects;

    private CustomizationMode currentMode = CustomizationMode.None;
    private Color chosenColor;
    [SerializeField] public Color[] colorsAvailable;
    [SerializeField] public Sprite[] plantSprites;
    private Sprite chosenPlant;
    bool hasPickedSeed, hasPickedColor;

    GlobalStateManager_ GSM;

    public Vector3 playerStartPos;
    GameObject player;
    [SerializeField] public Transform goBackPos;
    [SerializeField] public float otherMoveSpeed = 4;

    GlobalStateManager_ gsm;


    Vector3 posToGo;
    public bool isAtTarget;
    public bool standingStill;
    [SerializeField] Button[] closeMenuButton, openMenuButton;

    public bool isNotAtStartYPos;
    [SerializeField] public Animator playerAnimator;
    private void Awake()
    {
        gsm = GlobalStateManager_.Instance;
        /* allPlantObjects = GameObject.FindObjectsOfType<PlantObject_>();
         allColorObjects = GameObject.FindObjectsOfType<ChangeColorObject_>();
         plantIndexArray = new int[allPlantObjects.Length];
         colorIndexArray = new int[allColorObjects.Length];*/
        player = FindObjectOfType<Simon_FollowTarget_>().gameObject;
        playerStartPos = player.transform.position;
        changeColorObjects_ = FindObjectsOfType<ChangeColorObject_>();
        // UpdateData();
        ChangeGardenLevel(gsm.GetInt("garden_level"));
        /* for (int i = 0; i < gardenStateObject.Length; i++)
         {
             gardenStateObject[i].SetActive(true);
         }*/

    }
    // Start is called before the first frame update
    void Start()
    {


        goBackPos.position = new Vector3(0, player.transform.position.y, 0);

        GSM = GlobalStateManager_.Instance;
        clickableObjects = FindObjectsOfType<GardenClickableObject_>();
        UpdateObjects();


    }




    //method that activates and deactivates gameobjects and buttons depending on the value of GardenLevelState enum
    private void CheckGardenLevel()
    {
        foreach(ChangeColorObject_ changeColorObject in changeColorObjects_)
        {
            if (changeColorObject.isPlanter)
            {
                if(currentLevelState == GardenLevelState.Level1)
                {
                    changeColorObject.spriteRenderer.sprite = planterVersions[0];
                    changeColorObject.uiSpriteRenderer.sprite = planterVersions[0];
                }
                else
                {
                    changeColorObject.spriteRenderer.sprite = planterVersions[1];
                    changeColorObject.uiSpriteRenderer.sprite = planterVersions[1];
                }
            }
        }
        bool level1 = currentLevelState == GardenLevelState.Level1;
        bool level2 = currentLevelState == GardenLevelState.Level2;
        bool level3 = currentLevelState == GardenLevelState.Level3;
        //garden boxses and stuff
        gardenStateObject[0].SetActive(level1 || level2 || level3);
        gardenStateObject[1].SetActive(level2 || level3);
        gardenStateObject[2].SetActive(level3);
        //plants available
        plantLevelGroup[0].interactable = level1 || level2 || level3;
        plantLevelGroup[1].interactable = level2 || level3;
        plantLevelGroup[2].interactable = level3;

        //colors available
        colorLevelGroup[0].interactable = level1 || level2 || level3;
        colorLevelGroup[1].interactable = level2 || level3;
        colorLevelGroup[2].interactable = level3;

        //planks available
        plankLevelGroup[0].interactable = level2 || level3;
        plankLevelGroup[1].interactable = level3;

    }

    public void TestButton()
    {
        Debug.Log("CLICK BUYTTPM");
    }

    //to save all plant and colors 
    public void UpdateData()
    {
        for (int i = 0; i < plantIndexArray.Length; i++)
        {
            plantIndexArray[i] = allPlantObjects[i].GetPlantIndex();
            allPlantObjects[i].SetPlantIndex(plantIndexArray[i]);
        }
        for (int i = 0; i < colorIndexArray.Length; i++)
        {
            colorIndexArray[i] = allColorObjects[i].GetColorIndex();

        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < closeMenuButton.Length; i++)
        {
            closeMenuButton[i].interactable = standingStill;
            openMenuButton[i].enabled = standingStill;
        }

        /*  closeMenuButton[0].interactable = canCloseMenu;
          closeMenuButton[1].interactable = canCloseMenu;
          closeMenuButton[2].interactable = canCloseMenu;*/
        // gameSession.canMove = currentMode == CustomizationMode.None;
        CheckGardenLevel();

        GardenMovement();

        //gameSession.canMove = player.transform.position.y == goBackPos.position.y && currentMode == CustomizationMode.None;
        GSM.SetBool("canMove", player.transform.position.y == goBackPos.position.y && currentMode == CustomizationMode.None);

    }



    public void SetPosToGo(Vector3 pos)
    {
        posToGo = pos;
    }
    public bool IsAtTarget()
    {
        return player.transform.position == posToGo;
    }
    public bool StandingStill()
    {
        return standingStill;
    }
    private void GardenMovement()
    {

        isAtTarget = player.transform.position == posToGo;
        standingStill = isAtTarget || player.transform.position.y == goBackPos.position.y;
        isNotAtStartYPos = player.transform.position.y != playerStartPos.y;



        //  canCloseMenu = isAtTarget || player.transform.position.y == playerStartPos.y;

        if (currentMode == CustomizationMode.None)
        {
            FindObjectOfType<GardenMovePlayer_>().SetGoToTarget(false);
        }

        //player walks back to start position if menu closes and is not already back at position
        if (currentMode == CustomizationMode.None && player.transform.position.y != playerStartPos.y)
        {
            Vector3 moveBack = Vector3.MoveTowards(player.transform.position, playerStartPos, otherMoveSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(moveBack);
        }


        //   canShutDownMenu = !FindObjectOfType<GardenMovePlayer_>().IsAtTarget() && player.transform.position.y != goBackPos.position.y;
    }

    private void UpdateObjects()
    {

        if (currentMode != CustomizationMode.None && player.transform.position.y == goBackPos.position.y)
        {
            playerStartPos = player.transform.position;
        }
        foreach (GardenClickableObject_ clickableObject in clickableObjects)
        {
            clickableObject.UpdateObject();
        }

    }


    public CustomizationMode GetMode()
    {
        return currentMode;
    }

    public void ChangeMode(int mode)
    {
        if (mode == 1) currentMode = CustomizationMode.Plant;
        else if (mode == 2) currentMode = CustomizationMode.Color;
        else if (mode == 3) currentMode = CustomizationMode.Planks;
        else currentMode = CustomizationMode.None;
        UpdateObjects();


    }
    public void ChangeGardenLevel(int levelIndex)
    {
        if (levelIndex == 1) currentLevelState = GardenLevelState.Level1;
        else if (levelIndex == 2) currentLevelState = GardenLevelState.Level2;
        else if (levelIndex == 3) currentLevelState = GardenLevelState.Level3;
        gsm.SetInt("garden_level", levelIndex);
    }


    public enum GardenLevelState
    {
        Level1, Level2, Level3
    }
    public enum CustomizationMode
    {
        None, Plant, Color, Planks
    }

    public void ChoosePlank(int plankIndex)
    {
        if (plankIndex <= plankSprites.Length)
        {
            plankSpriteRenderer.sprite = plankSprites[plankIndex];
            plankUIFeedBackSpriteRenderer.sprite = plankSprites[plankIndex];
        }
    }


    int plantIndex, colorIndex;
    public int GetColorIndex()
    {
        return colorIndex;
    }
    public int GetPlantIndex()
    {
        return plantIndex;
    }
    public void ChoosePlant(int index)
    {
        if (index <= plantSprites.Length)
        {
            chosenPlant = plantSprites[index];
            hasPickedSeed = true;
            plantIndex = index;

        }


    }
    public void ChooseColor(int index)
    {
        if (index <= colorsAvailable.Length)
        {
            chosenColor = colorsAvailable[index];
            hasPickedColor = true;
            colorIndex = index;
        }
    }


    //Get variables

    public bool HasPickedSeed()
    {
        return hasPickedSeed;
    }
    public bool HasPickedColor()
    {
        return hasPickedColor;
    }
    public Color ChosenColor()
    {
        return chosenColor;
    }

    public Sprite ChosenPlant()
    {
        return chosenPlant;
    }
}

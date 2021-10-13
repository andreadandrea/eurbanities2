using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PinInteraction_ : MonoBehaviour
{
    
    private PinWalker_ walker;

    private Simon_LevelLoader levelLoader;
    
    public Transform consistentDist;
    
    [NonSerialized] public float checkdist;
    Collider2D[] preAllocatedColliderArray = new Collider2D[10];
    
    private List<PathList> growingPathHolder = new List<PathList>();
    private List<PathList> foundPathHolder = new List<PathList>();

    private readonly Stopwatch stopwatch = new Stopwatch();

    public bool drawLinesAllLines = true;
    private GlobalStateManager_ GSM;
    
    public GameObject changeMovementTypeButton;


    private void Awake()
    {
        levelLoader = Simon_LevelLoader.Instance;
        GSM = GlobalStateManager_.Instance;
        UpdateMovementTypeButton();
    }

    public void ToggleMovementType()
    {
        GSM.ToggleBool("mapTravelMode");
        UpdateMovementTypeButton();
    }

    private void UpdateMovementTypeButton()
    {
        changeMovementTypeButton.GetComponentInChildren<Text>().text = GSM.GetBool("mapTravelMode") ? "Info" : "Travel" + " mode";
    }
    

    public void onClick(Transform clickedPin)
    {
        PinInfo_ pinInfo = clickedPin.GetComponent<PinInfo_>();
        
        if(pinInfo.scene != "")
            GSM.SetBool($"mapLocation_{pinInfo.scene}_unlocked", pinInfo.unlocked);
        
        if (GSM.GetBool("mapTravelMode"))
        {
            Debug.Log( pinInfo.allInfo());
            return;
        }

        if (!pinInfo.unlocked)
        {
            Debug.Log("not unlocked yet");
            return;
        }
        
        walker = FindObjectOfType<PinWalker_>();
        //if the click was during a walk or if the clicked pin is the same as the current walker goal
        if (walker.walking || clickedPin == walker.pin) return;
        
        //screen calulation would probably be better, this is very placeholderish for now, easiest way i could do it
        checkdist = Vector2.Distance(consistentDist.GetChild(0).position, consistentDist.GetChild(1).position);

        
        
        //does this work huh?
        //checkdist = Screen.width / 15f;
        //math is hard...
        
        //make sure walker is ready to move
        walker.currentSubPin = CalculateClosestPinFromWalker();
        if (ReferenceEquals(walker.currentSubPin, null))
        {
            Debug.Log("currentsubpin is null, is the walker too far away from the first pin?");
            return;
        }
        
        walker.pin = clickedPin;
        
        string scene = walker.pin.GetComponent<PinInfo_>().scene;
        
        Debug.Log($"Started loading scene: {scene}");

        if (!ReferenceEquals(levelLoader, null))
        {
            GSM.resetPlayerPos(scene);
            StartCoroutine(levelLoader.LoadLevelFromMap(scene));
        }
        
        stopwatch.Restart();

        bool ifWeFoundAPath = CalculateEntirePath();
        
        stopwatch.Stop();
        
        if(ifWeFoundAPath)
            Debug.Log($"Path calculation took {stopwatch.ElapsedMilliseconds}ms");
    }

    private Transform CalculateClosestPinFromWalker()
    {
        //just return currentsubpin if there is one
        if (walker.currentSubPin != null) return walker.currentSubPin;
        
        //calculate new closest pin from the walker
        float dist = checkdist;
        Transform closest = null;
        
        //send out overlapcircle to get all the pins close to the walker
        int colliderAmount = Physics2D.OverlapCircleNonAlloc(walker.transform.position, checkdist, preAllocatedColliderArray, LayerMask.GetMask("Pins"));

        //simple loop through all colliders and get the closest one
        for(var i = 0; i < colliderAmount; i++)
        {
            Transform t = preAllocatedColliderArray[i].transform;
            
            float tempdist = Vector2.Distance(walker.transform.position, t.position);
            if (!(tempdist < dist)) continue;
            
            dist = tempdist;
            closest = t;
        }
        return closest;
    }
    
    private Transform[] PossiblePins(PathList currentList)
    {
        //pick last last element
        Transform currentPin = currentList.Last();
        
        //create new list and then send out an overlapcircle to see what pins we hit
        var possiblePinsList = new List<Transform>();
        int colliderAmount = Physics2D.OverlapCircleNonAlloc(currentPin.position, checkdist, preAllocatedColliderArray, LayerMask.GetMask("Pins"));

        //loop through all colliders that the circle collider hit
        for(var i = 0; i < colliderAmount; i++)     
        {
            Transform t = preAllocatedColliderArray[i].transform;
            
            //the pin is the goal - skip all other ways, this is the only way we want
            if (t == walker.pin)
            {
                possiblePinsList.Clear();
                possiblePinsList.Add(t);
                break;
            }
            
            //check if pin is found in any of the parent paths
            var foundInAParentPath = false;
            foreach (var parent in currentList.parents)
            {
                foundInAParentPath = parent.Contains(t);
                if (foundInAParentPath)
                    break;
            }
            
            //if the pin is the current pin
            //if the currentlist contains the pin
            //if the pin was found in a parent path
            //skip it - continue with next
            if(t == currentPin || currentList.Contains(t) || foundInAParentPath) continue;
            
            //skip all other goal transforms, you're not allowed to go there
            if (t.GetComponent<PinInfo_>().isGoal) continue;
            
            //any of these are possible pins which i can go to
            possiblePinsList.Add(t);
        }
        
        //sort it by distance, make it to an array and then return it
        return possiblePinsList.OrderBy(possiblePin =>
        {
            Vector3 currentPinPos = possiblePin.position;
            Vector3 walkerPos = walker.pin.position;
            Vector3 GoalPos = currentPin.position;

            float cost = Vector3.Distance(walkerPos, currentPinPos);
            cost += Vector3.Distance(GoalPos, currentPinPos);
            return cost;
        }).ToArray();
    }

    private bool CalculateEntirePath()
    {
        //Inital path, everything must have a beginning...
        var initialPathList = new PathList {walker.currentSubPin};
        growingPathHolder.Add(initialPathList);
        
        //recursion doom - bruteforce to find best way
        for (var recursions = 0; recursions < 50; recursions++)
        {
            //loop through all paths
            for(var listIndex = 0; listIndex < growingPathHolder.Count; listIndex++)
            {
                
                PathList currentPathList = growingPathHolder[listIndex];
                
                var possiblePins = PossiblePins(currentPathList);

                switch (possiblePins.Length)
                {
                    case 0:
                        //dead end - remove path
                        growingPathHolder.Remove(currentPathList);
                        break;
                    case 1:
                        //only one way to go let's go there
                        currentPathList.Add(possiblePins[0]);
                        
                        //this works but is very expensive, looking for a better approach
                        //currentPathList.increment -= possiblePins[0].GetComponent<PinInfo>().movementspeed;
                        break;
                    default:
                        //there is more than one way, lets split up
                        foreach (Transform possiblePin in possiblePins)
                        {
                            var tempList = new PathList(currentPathList) {possiblePin};
                            
                            //this works but is very expensive, looking for a better approach
                            //tempList.increment -= possiblePins[i].GetComponent<PinInfo>().movementspeed;
                            growingPathHolder.Add(tempList);
                        }
                        
                        //currentpath is now a parent and therfor can't grow anymore, so let's remove it
                        growingPathHolder.Remove(currentPathList);
                        break;
                }

                if (!currentPathList.Contains(walker.pin)) continue;

                //currentpath has found the goal!
                //let's remove it from growingPathHolder and add it to foundPathHolder!
                growingPathHolder.Remove(currentPathList);
                foundPathHolder.Add(currentPathList);
            }
        }
        
        
        //After recursion we loop through the found paths to find the path with the smallest cost
        float blockCount = 999;
        PathList finalList = null;
        foreach (var path in foundPathHolder)
        {
            float pathCost = path.GetCost();
            if (!(pathCost < blockCount)) continue;
            
            blockCount = pathCost;
            finalList = path;
        }
        
        //clear the path lists, cuz we dont need them anymore
        growingPathHolder.Clear();
        foundPathHolder.Clear();
        
        //If the best path is not null - a valid path has been found
        if (!ReferenceEquals(finalList, null))
        {
            //create a queue
            var finalQueue = new Queue<Transform>();
            foreach (var parent in finalList.parents)
            {
                //queue all parent pins into the queue
                parent.ForEach(o => finalQueue.Enqueue(o));
            }

            //clearing the parents list for performance reasons
            finalList.parents.Clear();

            //add all remaning current pins to the queue
            finalList.ForEach(o => finalQueue.Enqueue(o));
            
            //add queue to walker
            StartCoroutine(walker.SetPath(finalQueue));
            return true;
        }
        
        //the path is null - there is no valid path to the goal
        Debug.Log("Couldn't find reliable path");
        walker.walking = false;
        walker.pin = null;

        return false;
    }
    
    private void OnDrawGizmos()
    {

        //simple draw lines between pins, to make it easier to create a path
        if (!drawLinesAllLines || consistentDist == null) return;
        
        float tempdist = Vector2.Distance(consistentDist.GetChild(0).position, consistentDist.GetChild(1).position);
        
        #if UNITY_EDITOR
        Handles.color = Color.green;
        foreach (PinInfo_ pin in FindObjectsOfType<PinInfo_>())
        {
            Transform currentPin = pin.transform;
            var colliderArray = Physics2D.OverlapCircleAll(currentPin.position, tempdist, LayerMask.GetMask("Pins"));
            foreach (Collider2D allCollidedObjects in colliderArray)
            {
                Handles.DrawLine(currentPin.position, allCollidedObjects.transform.position, 5f);
            }
        }
        #endif
    }
}

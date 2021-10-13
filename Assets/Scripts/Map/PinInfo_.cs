using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class PinInfo_ : MonoBehaviour
{
    public bool isGoal = true;
    public bool unlocked = true;
    public string scene = "";
    public string info = "";

    public float movementspeed;
    
    private const float SineHeight = 0.05f;
    private float SineSpeed = 4.0f;
    private float SineCurrentIndex;
    
    public void Update()
    {
        if (!isGoal) return;
        
        SineCurrentIndex += Time.deltaTime;
        float y = SineHeight * Mathf.Sin (SineSpeed*SineCurrentIndex);
        transform.localPosition += new Vector3(0, y,0);
    }
    
    private void Start()
    {

        if (!isGoal) return;
        
        SineCurrentIndex = Random.Range(0, 10f);
    }

    public object allInfo()
    {
        return $"info: \"{info}\", unlocked: \"{unlocked}\", scene: \"{scene}\"";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PinInfo_)), CanEditMultipleObjects]
public class PinInfoEditor : Editor
{
    private Dictionary<string, SerializedProperty> pinInfo = new Dictionary<string, SerializedProperty>();
 
    protected virtual void OnEnable ()
    {
        pinInfo = new Dictionary<string, SerializedProperty>
        {
            {"isGoal", serializedObject.FindProperty("isGoal")},
            {"unlocked", serializedObject.FindProperty("unlocked")},
            {"scene", serializedObject.FindProperty("scene")},
            {"info", serializedObject.FindProperty("info")},
            {"movementspeed", serializedObject.FindProperty("movementspeed")}
        };
    }
    
    public override void OnInspectorGUI()
    {
        if (pinInfo.Count == 0) return;
        
        serializedObject.Update();
        
        string isGoalOrWaypoint = pinInfo["isGoal"].boolValue ? "Goal" : "Waypoint";
        
        
        var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
        pinInfo["isGoal"].boolValue = GUILayout.Toggle(pinInfo["isGoal"].boolValue, $"Pin type: {isGoalOrWaypoint}", style);
        
        if (pinInfo["isGoal"].boolValue)
        {
            string isUnlocked = pinInfo["unlocked"].boolValue ? "Yes" : "No";
            pinInfo["unlocked"].boolValue = GUILayout.Toggle(pinInfo["unlocked"].boolValue, $"Unlocked?: {isUnlocked}", style);
            
            EditorGUILayout.LabelField("Scene to load");
            pinInfo["scene"].stringValue = EditorGUILayout.TextField(pinInfo["scene"].stringValue);
            
            EditorGUILayout.LabelField("Information about place");
            pinInfo["info"].stringValue = EditorGUILayout.TextArea(pinInfo["info"].stringValue);
        }
        else
        {
            pinInfo["movementspeed"].floatValue = EditorGUILayout.Slider("Add movement speed?", pinInfo["movementspeed"].floatValue, -40, 20);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
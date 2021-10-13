using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Schema;
using FMODUnity;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalStateManager_ : MonoBehaviour
{
    public static GlobalStateManager_ Instance { get; private set; }
    private static Dictionary<string, int> localStateDict = new Dictionary<string, int>();
    private Dictionary<string, Vector3> playerPositions = new Dictionary<string, Vector3>();

    [SerializeField] private bool debug;
    
    public bool load;
    [SerializeField] private bool save;
    [SerializeField] private bool autoloadScene;
    
    [SerializeField] private int defaultScene = 2;
    private string path => $"{Application.dataPath}/Johannes";
    public string currentSave = "save1";
    private string fullPath => $"{path}/{currentSave}.e2s";

    private void Awake()
    {
        Debug.Log(fullPath);
        // Singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            if (load && CheckIfSaveExist(currentSave))
            {
                Debug.Log("loading save");
                Load();
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (!autoloadScene)
            {
                SetInt("currentScene", gameObject.scene.buildIndex);
                return;
            }

            SceneManager.LoadScene("MobileScene", LoadSceneMode.Additive);
            
            int sceneToLoad = GetInt("currentScene");
            sceneToLoad = defaultScene;
            
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            Debug.Log("DefaultSceneLoaded");
            
            SetInt("currentScene", sceneToLoad);

            SetBool("inDialog", false);
            SetBool("canMove", true);
            SetBool("pointAndClick", true);
        }
    }

    public bool CheckIfSaveExist(string sceneToCheck)
    {
        return File.Exists($"{path}/{sceneToCheck}.e2s");
    }
    
    public void SetBool(string str, bool value)
    {
        if(debug)
            Debug.Log($"GSM - {str} : {value}");
        localStateDict[str] = value ? 1 : 0;
        
        UpdateVisuals();
    }
    
    public void SetInt(string str, int value)
    { 
        if(debug)
            Debug.Log($"GSM - {str} : {value}");
        localStateDict[str] = value;
        
        UpdateVisuals();
    }
    
    public bool GetBool(string str)
    {
        if (str == "") return true;
        
        return localStateDict.ContainsKey(str) && localStateDict[str] == 1;
    }

    public int GetInt(string str)
    {
        if (str == "") return 0;
        
        return localStateDict.ContainsKey(str) ? localStateDict[str] : 0;
    }
    
    public void ToggleBool(string key)
    {
        localStateDict[key] = GetInt(key) == 1 ? 0 : 1;
        
        UpdateVisuals();
    }

    public void ChangeInt(string key, int change)
    {
        localStateDict[key] = GetInt(key) + change;
        
        UpdateVisuals();
    }
    
    public void SetPositionOfPlayer(string scene, Vector3 playerPos)
    {
        playerPositions[scene] = playerPos;
    }

    public Vector3 GetSpawnPointInThisScene(string gameObjectScene)
    {
        return playerPositions.ContainsKey(gameObjectScene) ? playerPositions[gameObjectScene] : Vector3.zero;
    }

    public void resetPlayerPos(string scene)
    {
        playerPositions.Remove(scene);
    }

    private void Save()
    {
        using var outputFile = File.Exists(fullPath) ? File.OpenWrite(fullPath) : File.Create(fullPath);
        //using var deflate = new DeflateStream(file, CompressionMode.Compress);
        using var writer = new BinaryWriter(outputFile);
        
        writer.Write(localStateDict.Count);
        foreach(var pair in localStateDict)
        {
            writer.Write(pair.Key);
            writer.Write(pair.Value);                    
        }
        
        writer.Write(playerPositions.Count);
        foreach(var pair in playerPositions)
        {
            writer.Write(pair.Key);
            writer.Write(pair.Value.x);
            writer.Write(pair.Value.y);
            writer.Write(pair.Value.z);
        }
    }

    private void Load()
    {
        using var inputFile = File.OpenRead(fullPath);
        //using var deflate = new DeflateStream(file, CompressionMode.Decompress);
        using var reader = new BinaryReader(inputFile);
        
        int progress = reader.ReadInt32();
        localStateDict = new Dictionary<string, int>(progress);
        while(progress-- > 0) {
            localStateDict.Add(reader.ReadString(), reader.ReadInt32());
        }
        
        progress = reader.ReadInt32();
        playerPositions = new Dictionary<string, Vector3>(progress);
        while(progress-- > 0)
        {
            playerPositions.Add(reader.ReadString(), new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
        }

        UpdateVisuals();
    }
    

    [Serializable]
    public struct State
    {
        public string key;
        public int value;
    }

    public State[] states;

    private void UpdateVisuals()
    {
        int count = localStateDict.Count;
        var stateArray = localStateDict.ToArray();
        states = new State[count];
        
        for (var i = 0; i < count; i++)
        {
            states[i].key = stateArray[i].Key;
            states[i].value = stateArray[i].Value;
        }
    }

    private void OnValidate()
    {
        foreach (var state in states)
        {
            localStateDict[state.key] = state.value;
        }
    }

    private void OnApplicationQuit()
    {
        if (!save) return;
        
        Debug.Log("save");
        
        Save();
    }
}

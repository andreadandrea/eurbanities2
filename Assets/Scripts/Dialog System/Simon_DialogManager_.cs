using Cinemachine;
using UnityEngine;
using System;

public class Simon_DialogManager_ : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCamera;
    [SerializeField] CinemachineVirtualCamera dialogCamera;

    public static event Action MainCameraActive;
    public static event Action DialogCameraActive;
    TextSystem_ textSystem => TextSystem_.textSystem;
    GlobalStateManager_ GSM = GlobalStateManager_.Instance;

    private void Awake()
    {
        Simon_FollowTarget_.DialogCamera += DialogCamera;
        Simon_FollowTarget_.MainCamera += MainCamera;
        Simon_FollowTarget_.StartDialog += StartDialogue;
    }
    private void OnDestroy()
    {
        Simon_FollowTarget_.DialogCamera -= DialogCamera;
        Simon_FollowTarget_.MainCamera -= MainCamera;
        Simon_FollowTarget_.StartDialog -= StartDialogue;
    }
    public void MainCamera()
    {
        MainCameraActive?.Invoke();
        dialogCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
    public void DialogCamera()
    {
        DialogCameraActive?.Invoke();
        dialogCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    public void StartDialogue(Dialog currentDialog)
    {
        if (ReferenceEquals(currentDialog, null)) return;
        textSystem.textBoard.SetActive(true);
        if (currentDialog.useRandomLines)
        {
            textSystem.ReadFileFor(currentDialog.file, currentDialog.randomLines[UnityEngine.Random.Range(0, currentDialog.randomLines.Length)]);
            return;
        }
        if (currentDialog.boolCheck != "" && GSM.GetBool(currentDialog.boolCheck))
            textSystem.ReadFileFor(currentDialog.file, currentDialog.altWantedLine);
        else
            textSystem.ReadFileFor(currentDialog.file, currentDialog.wantedLine);
    }
}
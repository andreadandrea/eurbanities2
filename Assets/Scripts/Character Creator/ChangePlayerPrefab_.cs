using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class ChangePlayerPrefab_ : MonoBehaviour
{
    GameObject woman => transform.GetChild(1).gameObject;
    GameObject man => transform.GetChild(0).gameObject;

    SpriteResolver womanFaceResolver => woman.transform.GetChild(0).GetChild(0).GetComponent<SpriteResolver>();
    SpriteResolver womanLeftHandResolver => woman.transform.GetChild(0).GetChild(1).GetComponent<SpriteResolver>();
    SpriteResolver womanRightHandResolver => woman.transform.GetChild(0).GetChild(6).GetComponent<SpriteResolver>();
    SpriteResolver womanTorsoResolver => woman.transform.GetChild(0).GetChild(10).GetComponent<SpriteResolver>();
    SpriteResolver manFaceResolver => man.transform.GetChild(0).GetChild(0).GetComponent<SpriteResolver>();
    SpriteResolver manLeftHandResolver => man.transform.GetChild(0).GetChild(1).GetComponent<SpriteResolver>();
    SpriteResolver manRightHandResolver => man.transform.GetChild(0).GetChild(6).GetComponent<SpriteResolver>();
    SpriteResolver manTorsoResolver => man.transform.GetChild(0).GetChild(10).GetComponent<SpriteResolver>();

    GlobalStateManager_ GSM;
    void Awake()
    {
        GSM = GlobalStateManager_.Instance;

        int color = GSM.GetInt("Character_Color");
        int currentFaceIndex = GSM.GetInt("Character_Face");

        if (GSM.GetInt("Character_Gender") == 0)
        {
            woman.gameObject.SetActive(true);
            man.gameObject.SetActive(false);

            womanFaceResolver.SetCategoryAndLabel("face", currentFaceIndex + color == 0 ? "face" : "face_" + (currentFaceIndex + (color * 4))); //Sets the face to the right index
            womanLeftHandResolver.SetCategoryAndLabel("Left_lowerArm", color == 0 ? "Left_lowerArm" : "Left_lowerArm_" + color);//Sets the lower left arm to the right index
            womanRightHandResolver.SetCategoryAndLabel("Right_lowerArm", color == 0 ? "Right_lowerArm" : "Right_lowerArm_" + color);//Sets the lower right arm to the right index
            womanTorsoResolver.SetCategoryAndLabel("Torso", color == 0 ? "Torso" : "Torso_" + color);//Sets the torso to the right index
        }
        else
        {
            woman.gameObject.SetActive(false);
            man.gameObject.SetActive(true);

            manFaceResolver.SetCategoryAndLabel("face", currentFaceIndex + color == 0 ? "face" : "face_" + (currentFaceIndex + (color * 4)));//Sets the face to the right index
            manLeftHandResolver.SetCategoryAndLabel("Left_lowerArm", color == 0 ? "Left_lowerArm" : "Left_lowerArm_" + color);//Sets the lower left arm to the right index
            manRightHandResolver.SetCategoryAndLabel("Right_lowerArm", color == 0 ? "Right_lowerArm" : "Right_lowerArm_" + color);//Sets the lower right arm to the right index
            manTorsoResolver.SetCategoryAndLabel("Torso", color == 0 ? "Torso" : "Torso_" + color);//Sets the torso to the right index
        }
    }
}

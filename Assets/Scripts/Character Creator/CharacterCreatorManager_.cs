using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;

public class CharacterCreatorManager_ : MonoBehaviour
{
    int currentFaceIndex = 0;
    int color = 0;
    bool gender = true;

    SpriteResolver womanFaceResolver => woman.transform.GetChild(0).GetChild(0).GetComponent<SpriteResolver>();
    SpriteResolver womanLeftHandResolver => woman.transform.GetChild(0).GetChild(1).GetComponent<SpriteResolver>();
    SpriteResolver womanRightHandResolver => woman.transform.GetChild(0).GetChild(6).GetComponent<SpriteResolver>();
    SpriteResolver womanTorsoResolver => woman.transform.GetChild(0).GetChild(10).GetComponent<SpriteResolver>();
    SpriteResolver manFaceResolver => man.transform.GetChild(0).GetChild(0).GetComponent<SpriteResolver>();
    SpriteResolver manLeftHandResolver => man.transform.GetChild(0).GetChild(1).GetComponent<SpriteResolver>();
    SpriteResolver manRightHandResolver => man.transform.GetChild(0).GetChild(6).GetComponent<SpriteResolver>();
    SpriteResolver manTorsoResolver => man.transform.GetChild(0).GetChild(10).GetComponent<SpriteResolver>();
    [SerializeField] Image[] faces;
    Vector2[] womanSizes = new Vector2[]
    {
        new Vector2(45.6f, 42.1f),
        new Vector2(32.8f,46.7f),
        new Vector2(38f,43.4f),
        new Vector2(38.2f,41.9f),
    };
    Vector2[] manSizes = new Vector2[]
    {
        new Vector2(32.1f, 47.9f),
        new Vector2(32.4f, 47.4f),
        new Vector2(32.4f, 48.6f),
        new Vector2(35.2f, 47.7f),
    };
    GameObject woman => transform.GetChild(0).gameObject;
    GameObject man => transform.GetChild(1).gameObject; //add the shit
    private void Start()
    {
        UpdateAssets();
        UpdateFaceSelection();
    }
    public void ChangeGender(bool gender)
    {
        if (gender)
        {
            woman.gameObject.SetActive(true);
            man.gameObject.SetActive(false);
            this.gender = true;
        }
        else
        {
            woman.gameObject.SetActive(false);
            man.gameObject.SetActive(true);

            this.gender = false;
        }
        UpdateAssets();
        UpdateFaceSelection();
    }
    public void SwitchColor(int index)
    {
        if (color == index) return;
        color = index;
        UpdateAssets();
        UpdateFaceSelection();
    }
    public void SwitchFaceIndex(int index)
    {
        if (currentFaceIndex == index) return;
        currentFaceIndex = index;
        UpdateAssets();
    }
    public void UpdateAssets() //this gets called after each change to update the visuals on the character to represent your choices
    {
        if (gender)
        {
            womanFaceResolver.SetCategoryAndLabel("face", currentFaceIndex + color == 0 ? "face" : "face_" + (currentFaceIndex + (color * 4))); //Sets the face to the right index
            womanLeftHandResolver.SetCategoryAndLabel("Left_lowerArm", color == 0 ? "Left_lowerArm" : "Left_lowerArm_" + color);//Sets the lower left arm to the right index
            womanRightHandResolver.SetCategoryAndLabel("Right_lowerArm", color == 0 ? "Right_lowerArm" : "Right_lowerArm_" + color);//Sets the lower right arm to the right index
            womanTorsoResolver.SetCategoryAndLabel("Torso", color == 0 ? "Torso" : "Torso_" + color);//Sets the torso to the right index
        }
        else
        {
            manFaceResolver.SetCategoryAndLabel("face", currentFaceIndex + color == 0 ? "face" : "face_" + (currentFaceIndex + (color * 4)));//Sets the face to the right index
            manLeftHandResolver.SetCategoryAndLabel("Left_lowerArm", color == 0 ? "Left_lowerArm" : "Left_lowerArm_" + color);//Sets the lower left arm to the right index
            manRightHandResolver.SetCategoryAndLabel("Right_lowerArm", color == 0 ? "Right_lowerArm" : "Right_lowerArm_" + color);//Sets the lower right arm to the right index
            manTorsoResolver.SetCategoryAndLabel("Torso", color == 0 ? "Torso" : "Torso_" + color);//Sets the torso to the right index
        }
    }
    public void UpdateFaceSelection()
    {
        if (gender)
        {
            for (int i = 0; i < faces.Length; i++)
            {
                womanFaceResolver.SetCategoryAndLabel("face", i + color == 0 ? "face" : "face_" + (i + (color * 4)));
                faces[i].sprite = womanFaceResolver.GetComponent<SpriteRenderer>().sprite;
                faces[i].GetComponent<RectTransform>().sizeDelta = womanSizes[i];
            }
            womanFaceResolver.SetCategoryAndLabel("face", currentFaceIndex + color == 0 ? "face" : "face_" + (currentFaceIndex + (color * 4)));
        }
        else
        {
            for (int i = 0; i < faces.Length; i++)
            {
                manFaceResolver.SetCategoryAndLabel("face", i + color == 0 ? "face" : "face_" + (i + (color * 4)));
                faces[i].sprite = manFaceResolver.GetComponent<SpriteRenderer>().sprite;
                faces[i].GetComponent<RectTransform>().sizeDelta = manSizes[i];
            }
            manFaceResolver.SetCategoryAndLabel("face", currentFaceIndex + color == 0 ? "face" : "face_" + (currentFaceIndex + (color * 4)));
        }
    }
    public void SaveCharacter() //add whatever function to the end of this to make it work
    {
        GlobalStateManager_.Instance.SetInt("Character_Gender", gender ? 0 : 1);
        GlobalStateManager_.Instance.SetInt("Character_Color", color);
        GlobalStateManager_.Instance.SetInt("Character_Face", currentFaceIndex);
    }
}

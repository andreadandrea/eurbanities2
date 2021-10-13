using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class FunnyTextHandler_ : MonoBehaviour
{
    #region variables
    Settings_ settings => Settings_.settings;
    
    
    string hexCode = "#000000";
    TextInfoKeeper_ Tik => TextInfoKeeper_.tik;

    public static FunnyTextHandler_ FTH;
    
    char functionChar;
    readonly string charsWithFunctions = "{}<>[]&+-*\\#/$";
    string collectedCharsForInfo = "";

    bool tryingToAddFunction = false, nextCharacter = false, adCharToList = false, collectForInfo = false, jumpOverNextChar = false;
    bool puttingOutAnImage = false, addNextWordWhole = false, pauseWriting = false;
    public bool addWrintingInstantly = false, occupied = false;
    float timer = 0, xOffset = 0, yOffset = 0/*, previousYOffset = 0*/, boxSize = 0, boxPadding = 75, textSize = 2f, previousTextSize = 2;
    [SerializeField][Range(0.01f, 0.1f)] float textSpeed;

    private int spaceOffset = 25, newlineOffset = 100;

    private List<GameObject> currentWord = new List<GameObject>();
    private List<GameObject> currentRow = new List<GameObject>();
    private List<GameObject> affectedChars = new List<GameObject>();
    public GameObject charImage, InfoOrEaseOfReading;
    #endregion
    private void Awake()
    {
        if (FTH == null) FTH = this;
        else Destroy(gameObject);
    }
    public void ClearText()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //change the game state after this to make the player able to move again(if that is a restriction)
    }
    public bool CanISendNextText()
    {
        addWrintingInstantly = occupied;
        return !occupied;
    }
    public IEnumerator AddTheNextCharacter(char character)
    {
        puttingOutAnImage = true;
        GameObject image = Instantiate(charImage, transform);
        if (adCharToList) affectedChars.Add(image);
        currentWord.Add(image);

        int extraYOffset = 0;

        if (charsToMove.ContainsKey(character))
        {
            extraYOffset = Mathf.RoundToInt(charsToMove[character]);
        }
        if (previousTextSize > textSize)
        {
            float newlyAddedYoffset = (newlineOffset / textSize) - (newlineOffset / previousTextSize);
            yOffset -= newlyAddedYoffset;
            previousTextSize = textSize;
            foreach (GameObject characterImage in currentRow)
            {
                characterImage.transform.localPosition = characterImage.transform.localPosition + new Vector3(0, -newlyAddedYoffset);
            }
        }

        currentRow.Add(image);
        CharacterPhysicalFunctionScript_ imageScript = image.GetComponent<CharacterPhysicalFunctionScript_>();
        Image imageImage = image.GetComponent<Image>();

        Sprite sprite = Tik.GetSprite(character);

        imageImage.color = SetColor();

        imageImage.sprite = sprite;

        image.GetComponent<RectTransform>().sizeDelta = sprite.rect.size / textSize;

        int width = Mathf.RoundToInt(sprite.rect.size.x / textSize);//tik.GetWidth(sprite) / (Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x) / 2) ;

        //imageImage.sprite = SetCharacter(character);
        image.transform.localPosition = new Vector2(xOffset + (width / 2), yOffset + (extraYOffset / textSize));


        if (Settings_.settings.AnimatedText && physicalApperance != PhysicalApperance.stationary)
        {
            imageScript.enabled = true;
            imageScript.physicalApperance = physicalApperance;
        }

        if (adCharToList) boxSize += width + textSize;
        /*instantiate the picture and offsett it with currentoffset + half of the width of the current picture*/
        xOffset += width + 2;
        RectTransform targetRect = GetComponent<RectTransform>();
        if (xOffset > (targetRect.sizeDelta.x / 2) - boxPadding)
        {
            xOffset = (-targetRect.sizeDelta.x / 2) + boxPadding;
            yOffset -= newlineOffset / textSize;
            if (-yOffset >= 0)
            {
                pauseWriting = true;
                //makeButtonToResumeVisible();
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Backspace));
                RemoveTextAndContinue(character);
            }
            for (int i = 0; i < currentWord.Count; i++)
            {
                extraYOffset = 0;

                width = Mathf.RoundToInt(currentWord[i].GetComponent<Image>().rectTransform.sizeDelta.x);
                currentWord[i].transform.localPosition = new Vector2(xOffset + (width / 2), yOffset + extraYOffset);
                xOffset +=currentWord[i].GetComponent<Image>().sprite.rect.width / textSize + 2;
                currentWord[i].GetComponent<CharacterPhysicalFunctionScript_>().UpdatePos();
                if (charsToMove.ContainsKey(character))
                {
                    extraYOffset = Mathf.RoundToInt(charsToMove[character] / textSize);
                }
            }
        }

        puttingOutAnImage = false;
    }
    public Color SetColor()
    {
        try
        {
            ColorUtility.TryParseHtmlString(hexCode, out Color color);
            return color;
        }
        catch
        {
            return Color.black;
        }
    }
    public IEnumerator WritheTheStuff(string text)
    {
        if (occupied) yield break;
        occupied = true;
        for (int i = 0; i < transform.childCount; i++) { Destroy(transform.GetChild(i).gameObject); }
        /*StreamReader stream = new StreamReader(path);
        string inputText = stream.ReadToEnd();*/
        if (text == "") text = "ERROR!";
        #region reseting variables
        currentRow.Clear();
        currentWord.Clear();
        affectedChars.Clear();
        textSize = 2;
        physicalApperance = PhysicalApperance.stationary;
        hexCode = "#000000";
        #endregion
        yOffset = GetComponent<RectTransform>().sizeDelta.y - boxPadding;
        xOffset = (-GetComponent<RectTransform>().sizeDelta.x / 2) + boxPadding; //C: \Users\User\Desktop\Eurbanities textFiles\sv_SV\Characters\Girl\Town hall
        int currentSpotInText = -1;
        foreach (char character in text)
        {
            currentSpotInText++;
            if (jumpOverNextChar)
            {
                //Debug.Log($"{character} was jumped over");
                jumpOverNextChar = false;
                continue;
            }
            if (collectionsForFunctions != CollectionsForFunctions.black)
            {
                switch (collectionsForFunctions)
                {
                    default: break;
                    case CollectionsForFunctions.Color:
                        if (character == ' ')
                        {
                            collectionsForFunctions = CollectionsForFunctions.black;
                            tryingToAddFunction = false;
                        }
                        else hexCode += character;
                        break;
                    case CollectionsForFunctions.Info:
                        if (collectForInfo == true)
                        {
                            if (character == '"') collectForInfo = false;
                            else collectedCharsForInfo += character;
                            continue;
                        }
                        /*else if (character == ' ' || character == '\r')
                        {
                            if (character == '\r') currentRow.Clear();
                            addNextWordWhole = false;
                            yield return new WaitUntil(() => !puttingOutAnImage);
                            currentWord.Clear();
                            xOffset += spaceOffset;
                            continue;
                        }*/
                        else if (character == ']')
                        {
                            collectionsForFunctions = CollectionsForFunctions.black;
                            AddOverlayToWord();
                            collectedCharsForInfo = "";
                            adCharToList = false;
                            affectedChars.Clear();
                            collectForInfo = false;
                            tryingToAddFunction = false;
                        }
                        else
                        {
                            if (!addNextWordWhole || !addWrintingInstantly) yield return new WaitUntil(() => nextCharacter);
                            StartCoroutine(AddTheNextCharacter(character));
                            nextCharacter = false;
                        }
                        break;
                }
                continue;
            }
            if (character == ' ' && !tryingToAddFunction)
            {
                yield return new WaitUntil(() => !puttingOutAnImage);
                currentWord.Clear();
                xOffset += spaceOffset / textSize;
                addNextWordWhole = false;
                continue;
            }if (character == '\r')
            {
                previousTextSize = textSize;
                addNextWordWhole = false;
                jumpOverNextChar = true;
                yield return new WaitUntil(() => !puttingOutAnImage);
                currentWord.Clear();
                currentRow.Clear();
                yOffset -= newlineOffset / textSize;
                xOffset = (-GetComponent<RectTransform>().sizeDelta.x / 2) + 100;
                if (-yOffset >= 0)
                {
                    pauseWriting = true;
                    //makeButtonToResumeVisible();
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Backspace));
                    RemoveTextAndContinue(character);
                }
                continue;
            }
            if (!tryingToAddFunction)
            {
                if (charsWithFunctions.Contains(character.ToString()))
                {
                    tryingToAddFunction = true;
                    functionChar = character;
                    continue;
                }
            }
            if (!tryingToAddFunction)
            {
                StartCoroutine(AddTheNextCharacter(character));
                if (!addNextWordWhole && !addWrintingInstantly) yield return new WaitUntil(() => nextCharacter);
                nextCharacter = false;
                if (!addNextWordWhole && !addWrintingInstantly) yield return new WaitUntil(() => !pauseWriting);
                continue;
            }
            switch (functionChar)
            {
                default:
                            Debug.Log("you did goofed my guy (as in i as the programmer did goofed, not you as in the writer)");
                            break;
                case '{':
                            //this will ad a function that will change the color of the text in some way
                            hexCode = character.ToString();
                            collectionsForFunctions = CollectionsForFunctions.Color;
                            break;
                case '}':
                            //this will end the effect of the above function
                            hexCode = "#000000";
                            if (character != ' ')
                            {
                                if (charsWithFunctions.Contains(character.ToString()))
                                {
                                    tryingToAddFunction = true;
                                    functionChar = character;
                                    if (!addWrintingInstantly) yield return new WaitUntil(() => !pauseWriting);
                                    continue;
                                }
                                if (!addWrintingInstantly)  yield return new WaitUntil(() => !puttingOutAnImage);
                                StartCoroutine(AddTheNextCharacter(character));
                                nextCharacter = false;
                                currentWord.Clear();
                            }
                            else
                            {
                                addNextWordWhole = false;
                                xOffset += spaceOffset / textSize;
                            }
                            break;
                case '<':
                            //this will add a functino that will change the physical apperance of the text is some way
                            switch (character)
                            {
                                default:
                                case 'w':
                                        physicalApperance = PhysicalApperance.wiggle; 
                                        break;
                                case 's':
                                        physicalApperance = PhysicalApperance.shake;
                                        break;
                            }
                            adCharToList = true;
                            break;
                case '>':
                            //this will end the effect of the above function
                            physicalApperance = PhysicalApperance.stationary;
                            if (character != ' ') 
                            {
                                if (charsWithFunctions.Contains(character.ToString()))
                                {
                                    AddOverlayToWord();
                                    tryingToAddFunction = true;
                                    functionChar = character;
                            if (!addWrintingInstantly) yield return new WaitUntil(() => !pauseWriting);
                                    continue;
                                }
                        if (!addWrintingInstantly) yield return new WaitUntil(() => !puttingOutAnImage);
                                StartCoroutine(AddTheNextCharacter(character));
                                nextCharacter = false;
                                currentWord.Clear();
                            }
                            else
                            {
                                addNextWordWhole = false;
                                xOffset += spaceOffset / textSize;
                            }
                            AddOverlayToWord();
                            break;
                case '[':
                            //the text that is before a quote is going to be added as en explenation to the word(s) after it!
                            //aslong as its all within the brackets
                            collectedCharsForInfo += character;
                            collectionsForFunctions = CollectionsForFunctions.Info;
                            collectForInfo = true;
                            adCharToList = true;
                            break;
                case ']':
                            xOffset += 10;
                            break;
                case '&':
                            if (character != ' ')
                            {
                                if (charsWithFunctions.Contains(character.ToString()))
                                {
                                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                                    tryingToAddFunction = true;
                                    functionChar = character;
                                    if (!addWrintingInstantly) yield return new WaitUntil(() => !pauseWriting);
                                    continue;
                                }
                        if (!addWrintingInstantly) yield return new WaitUntil(() => !puttingOutAnImage);
                                //RemoveTextAndContinue(character);
                                nextCharacter = false;
                                currentWord.Clear(); 
                                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                                RemoveTextAndContinue(character);
                                StartCoroutine(AddTheNextCharacter(character));
                            }
                            else
                            {
                                addNextWordWhole = false;
                                xOffset += spaceOffset / textSize;
                                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                                RemoveTextAndContinue(character);
                            }
                            break;
                case '#':
                            //this will change the size of the next character, if nothing is before the current one, the normal size will be used
                            addNextWordWhole = true;
                            if (character != ' ')
                            {
                                if (charsWithFunctions.Contains(character.ToString()))
                                {
                                    tryingToAddFunction = true;
                                    functionChar = character;
                                    if (!addWrintingInstantly) yield return new WaitUntil(() => !pauseWriting);
                                    continue;
                                }
                                if (!addWrintingInstantly) yield return new WaitUntil(() => !puttingOutAnImage);
                                StartCoroutine(AddTheNextCharacter(character));
                                nextCharacter = false;
                                currentWord.Clear();
                            }
                            else xOffset += spaceOffset / textSize;
                            break;
                case '/':
                            //this will add a pause that last aslong as the number you write
                            /*float timer = (int)character;*/
                            if (timeConversion.ContainsKey(character)) yield return new WaitForSeconds(timeConversion[character]);
                            break;
                case '*':
                            //this will change the textSpeed
                            if (timeConversion.ContainsKey(character)) textSpeed = timeConversion[character] / 10;
                            break;
                case '+':
                            //this will change the size of the next characters in a negative manner
                            if (timeConversion.ContainsKey(character)) textSize = Mathf.Clamp(textSize -= timeConversion[character], 0.5f, 4);
                            else textSize = 2;
                            break;
                case '-':
                            //this will change the size of the next characters in a positive manner
                            if (timeConversion.ContainsKey(character)) textSize = Mathf.Clamp(textSize += timeConversion[character], 0.5f, 4);
                            else textSize = 2;  
                            break;
                case '\\':
                            //this will add the next character even if it would regularly be a function starter/stopper
                            StartCoroutine(AddTheNextCharacter(character));
                            break;
                case '$':
                            //adds the player name to the converstaion
                            foreach (char extraCharacter in "player name")
                            {
                                if (extraCharacter == ' ')
                                {
                                    xOffset += spaceOffset / textSize;
                                    continue;
                                }
                                if (!addWrintingInstantly) yield return new WaitUntil(() => nextCharacter);
                                StartCoroutine(AddTheNextCharacter(extraCharacter));
                                nextCharacter = false;
                            }
                            break;
            }
            tryingToAddFunction = false;
            if (!addWrintingInstantly) yield return new WaitUntil(() => !pauseWriting);
        }
        occupied = false;
    }
    void RemoveTextAndContinue(char character)
    {
        previousTextSize = textSize;
        xOffset = (-GetComponent<RectTransform>().sizeDelta.x / 2) + 100;
        yOffset = GetComponent<RectTransform>().sizeDelta.y - (boxPadding / textSize);
        pauseWriting = false;
        int ChildCount = transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            if (currentWord.Contains(transform.GetChild(ChildCount - i - 1).gameObject)) continue;
            Destroy(transform.GetChild(ChildCount - i - 1).gameObject);
        }

        for (int i = 0; i < currentWord.Count; i++)
        {
            int extraYOffset = 0;

            if (charsToMove.ContainsKey(character))
            {
                extraYOffset = charsToMove[character];
            }
            int width = Mathf.RoundToInt(currentWord[i].GetComponent<Image>().rectTransform.sizeDelta.x);
            currentWord[i].transform.localPosition = new Vector2(transform.position.x + xOffset + width / 2, transform.position.y + 300 + yOffset + extraYOffset);
            xOffset += currentWord[i].GetComponent<Image>().sprite.rect.width / 2 + 2;
        }

    }
    void AddOverlayToWord()
    {
        if (affectedChars.Count < 1)
        {
            Debug.Log("the chamber is empty sir!");
            return;
        }
        adCharToList = false;

        GameObject box = Instantiate(InfoOrEaseOfReading, transform);

        box.GetComponent<RectTransform>().sizeDelta = new Vector2(boxSize, 50 / textSize);

        box.transform.localPosition = affectedChars[0].transform.localPosition -
            new Vector3((affectedChars[0].transform.localPosition.x - affectedChars[affectedChars.Count - 1].transform.localPosition.x) / 2, 0);
        //add the affected chars to the function in the script that is on the box that have yet to be made! :DONE:

        PauseCharPhysicalFunctions_ pauseFunction = box.GetComponent<PauseCharPhysicalFunctions_>();

        for (int i = 0; i < affectedChars.Count; i++) pauseFunction.charsToEffect.Add(affectedChars[i].GetComponent<CharacterPhysicalFunctionScript_>());

        if (collectedCharsForInfo != "")
        {
            pauseFunction.hasAMessage = true;
            pauseFunction.infoMessage = collectedCharsForInfo;
        }
        boxSize = 0;
        affectedChars.Clear();
    }
    private void Update()
    {
        timer += Time.deltaTime / textSpeed;
        if (addWrintingInstantly) nextCharacter = true;
        else if (timer > 1)
        {
            timer -= 1;
            nextCharacter = true;
        }
    }
    PhysicalApperance physicalApperance = PhysicalApperance.stationary;
    CollectionsForFunctions collectionsForFunctions = CollectionsForFunctions.black;
    /*public enum VisualApperance
    {
        rainbow,
        red,
        green,
        yellow,

        black,//this is the basic/neutral option
    }*/
    public enum PhysicalApperance
    {
        wiggle,
        shake,

        stationary,//this is the basic/neutral option
    }
    public enum CollectionsForFunctions
    {
        Color,
        Info,
        black,
    }
    readonly Dictionary<char, int> charsToMove = new Dictionary<char, int> //making sure that the text will get instatiated in the right spot
    {
        {'Ą', -10},
        {'ą', -10},
        {'Ș', -10},
        {'ș', -10},
        {'Ț', -10},
        {'ț', -10},
        {'Ę', -10},
        {'ę', -10},
        {'g', -18},
        {'p', -14},
        {'q', -12},
        {'j', -16},
        {'Q', -8},
        {'y', -17},
        {',', -5},
        {'*', 31},
        {'=', 15},
        {'^', 20},
        {'~', 15},
        {'-', 15},
        {'.', -2},
        {'\'', 31},
    };
    readonly Dictionary<char, float> timeConversion = new Dictionary<char, float> //this might be redundant
    {
        {'9', 0.9f},
        {'8', 0.8f},
        {'7', 0.7f},
        {'6', 0.6f},
        {'5', 0.5f},
        {'4', 0.4f},
        {'3', 0.3f},
        {'2', 0.2f},
        {'1', 0.1f},
        {'0', 1f},
    };
}

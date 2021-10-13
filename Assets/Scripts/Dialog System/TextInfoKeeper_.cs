using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInfoKeeper_ : MonoBehaviour
{
    public static TextInfoKeeper_ tik;
    public Sprite[] charSprites;
    Dictionary<char, Sprite> charSpriteDictionary; 
    private void Awake()
    {
        charSpriteDictionary = new Dictionary<char, Sprite>
    {
        {'a', charSprites[0]},
        {'b', charSprites[1]},
        {'c', charSprites[2]},
        {'d', charSprites[3]},
        {'e', charSprites[4]},
        {'f', charSprites[5]},
        {'g', charSprites[6]},
        {'h', charSprites[7]},
        {'i', charSprites[8]},
        {'j', charSprites[9]},
        {'k', charSprites[10]},
        {'l', charSprites[11]},
        {'m', charSprites[12]},
        {'n', charSprites[13]},
        {'o', charSprites[14]},
        {'p', charSprites[15]},
        {'q', charSprites[16]},
        {'r', charSprites[17]},
        {'s', charSprites[18]},
        {'t', charSprites[19]},
        {'u', charSprites[20]},
        {'v', charSprites[21]},
        {'w', charSprites[22]},
        {'x', charSprites[23]},
        {'y', charSprites[24]},
        {'z', charSprites[25]},
        {'å', charSprites[26]},
        {'ä', charSprites[27]},
        {'ö', charSprites[28]},
        {'A', charSprites[29]},
        {'B', charSprites[30]},
        {'C', charSprites[31]},
        {'D', charSprites[32]},
        {'E', charSprites[33]},
        {'F', charSprites[34]},
        {'G', charSprites[35]},
        {'H', charSprites[36]},
        {'I', charSprites[37]},
        {'J', charSprites[38]},
        {'K', charSprites[39]},
        {'L', charSprites[40]},
        {'M', charSprites[41]},
        {'N', charSprites[42]},
        {'O', charSprites[43]},
        {'P', charSprites[44]},
        {'Q', charSprites[45]},
        {'R', charSprites[46]},
        {'S', charSprites[47]},
        {'T', charSprites[48]},
        {'U', charSprites[49]},
        {'V', charSprites[50]},
        {'W', charSprites[51]},
        {'X', charSprites[52]},
        {'Y', charSprites[53]},
        {'Z', charSprites[54]},
        {'Å', charSprites[55]},
        {'Ä', charSprites[56]},
        {'Ö', charSprites[57]},
        {',', charSprites[58]},
        {'.', charSprites[59]},
        {'\'', charSprites[60]},
        {'#', charSprites[61]},
        {'$', charSprites[62]},
        {'%', charSprites[63]},
        {'&', charSprites[64]},
        {'(', charSprites[65]},
        {')', charSprites[66]},
        {'*', charSprites[67]},
        {'=', charSprites[68]},
        {'@', charSprites[69]},
        {'[', charSprites[70]},
        {']', charSprites[71]},
        {'^', charSprites[72]},
        {'<', charSprites[73]},
        {'>', charSprites[74]},
        {'_', charSprites[75]},
        {'{', charSprites[76]},
        {'}', charSprites[77]},
        {'~', charSprites[78]},
        {'£', charSprites[79]},
        {'¤', charSprites[80]},
        {'Á', charSprites[81]},
        {'Â', charSprites[82]},
        {'É', charSprites[83]},
        {'Í', charSprites[84]},
        {'Î', charSprites[85]},
        {'Ó', charSprites[86]},
        {'Ú', charSprites[87]},
        {'á', charSprites[88]},
        {'â', charSprites[89]},
        {'é', charSprites[90]},
        {'í', charSprites[91]},
        {'î', charSprites[92]},
        {'ó', charSprites[93]},
        {'ú', charSprites[94]},
        {'ü', charSprites[95]},
        {'Ą', charSprites[96]},
        {'ą', charSprites[97]},
        {'Ć', charSprites[98]},
        {'ć', charSprites[99]},
        {'Ę', charSprites[100]},
        {'ę', charSprites[101]},
        {'Ń', charSprites[102]},
        {'ń', charSprites[103]},
        {'Ś', charSprites[104]},
        {'ś', charSprites[105]},
        {'Ü', charSprites[106]},
        {'§', charSprites[107]},
        {'Ź', charSprites[108]},
        {'ź', charSprites[109]},
        {'Ș', charSprites[110]},
        {'ș', charSprites[111]},
        {'Ț', charSprites[112]},
        {'ț', charSprites[113]},
        {'€', charSprites[114]},
        {'!', charSprites[115]},
        {'ß', charSprites[116]},
        {'-', charSprites[117]},
        {'?', charSprites[118]},
        //
    };
        if (tik == null) tik = this;
    }
    public Sprite GetSprite(char character)
    {
        if (charSpriteDictionary.ContainsKey(character))
            return charSpriteDictionary[character];
        return charSprites[118];
    }
    public int GetWidth(Sprite sprite)
    {
        int width = sprite.texture.width;
        int returnWidth = sprite.texture.width;
        int height = sprite.texture.height;
        bool FoundStart = false;
        bool foundEnd = false;
        for (int i = 0; i < width; i++)
        {
            if (FoundStart && foundEnd) return returnWidth;
            for (int a = 0; a < height; a++)
            {
                if (!FoundStart)
                {
                    Color pixle = sprite.texture.GetPixel(i, a);
                    if (pixle == Color.black)
                    {
                        returnWidth -= i;
                        FoundStart = true;
                    }
                }
                if (!foundEnd)
                {
                    Color pixle = sprite.texture.GetPixel(width - i, height - a);
                    if (pixle == Color.black)
                    {
                        returnWidth -= i;
                        foundEnd = true;
                    }
                }
            }
        }
        Debug.Log("something aint right chef!");
        return 0;
    }
}

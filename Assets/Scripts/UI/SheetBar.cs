using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetBar : MonoBehaviour {
    public Image[] sheetImages;
    public Sprite sheetSprite;

    public int sheetIndex = 0;

    public void catchSheet()
    {
        sheetImages[sheetIndex].sprite = sheetSprite;
        Color imageColor = sheetImages[sheetIndex].color;
        imageColor.a = 1;

        sheetImages[sheetIndex].color = imageColor;
        sheetIndex++;
    }
}

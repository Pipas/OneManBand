using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetBar : MonoBehaviour {
    public GameObject[] sheets;
    public int sheetIndex = 0;

    public void catchSheet()
    {
        if (sheetIndex < sheets.Length)
        {
            sheets[sheetIndex].SetActive(false);
            sheetIndex++;
            sheets[sheetIndex].SetActive(true);
        }
    }

    public int getSheetIndex()
    {
        return sheetIndex;
    }
}

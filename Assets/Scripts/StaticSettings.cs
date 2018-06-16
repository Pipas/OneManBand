using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSettings : MonoBehaviour {
    public const int EASY_MARGIN = 200;
    public const int MEDIUM_MARGIN = 150;
    public const int HARD_MARGIN = 125;

    public const string EASY = "easy";
    public const string MEDIUM = "medium";
    public const string HARD = "hard";
    
    public static string difficulty = "medium";       // easy, medium or hard
    public static float volumeBGM = 1.0f;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static void changeDifficulty(string diff) {
        difficulty = diff;
    }

    public static void changeVolumeBGM(float volume) {
        volumeBGM = volume;
    }

    public static int setSkillMargin()
    {
        if (difficulty == EASY)
        {
            return EASY_MARGIN;
        }
        else if (difficulty == MEDIUM)
        {
            return MEDIUM_MARGIN;
        } else {
            return HARD_MARGIN;
        }
    }
}

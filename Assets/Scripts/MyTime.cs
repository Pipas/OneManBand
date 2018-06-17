using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyTime  {

    // returns elapsed time and updates previous time
    public static long ElapsedTime(long previousTime)
    {
        long currentTime = CurrentTimeMillis();
        long elapsedTime = 0;
        if (previousTime != 0)
        {
            elapsedTime = currentTime - previousTime;
        }
        return elapsedTime;
    }


    public static long CurrentTimeMillis()
    {
        return (long) (Time.timeSinceLevelLoad * 1000);
    }
}

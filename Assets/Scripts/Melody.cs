using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody : MonoBehaviour {

	/* --- Inspector --- */

	public string rythem;
	public float fadeTime;
	public long timeWindow;


	/* --- Attrs --- */

	private AudioSource sound;
	private bool playing;
	private bool waiting;
	private long waitingStartTime;
	private float DEFAULT_VOLUME;


	/* --- Methods --- */

    void Start() {
    
		sound = gameObject.GetComponent<AudioSource>();
		DEFAULT_VOLUME = sound.volume;
		playing = false;
		waiting = false;
        waitingStartTime = 0;
	}

    public void FadeIn()
    {
		if ((!playing && !waiting) || (waiting && MyTime.ElapsedTime(waitingStartTime) >= timeWindow))
        {
            Play();
        }

        if (!waiting && playing && sound.volume < 1)
        {
            sound.volume = sound.volume + (Time.deltaTime / (fadeTime + 0.1f));
        }
		
		if (!waiting && !sound.isPlaying)
		{
            Wait();
		}
    }

    public void FadeOut()
    {
		if (sound.volume > DEFAULT_VOLUME)
		{
			sound.volume = sound.volume - (Time.deltaTime / (fadeTime + 0.1f));
		}
		else
		{
			Stop();
		}
    }

	public void Play()
	{
		if (!playing && (int) Time.time % 4 == 0)
		{
			playing = true;
            waiting = false;
            sound.Play();
		}		
	}

	public void Stop()
	{
		if (playing)
		{
			playing = false;
			waiting = false;
            sound.Stop();
		}
	}

	public void Wait()
	{
		Stop();
        waiting = true;
        waitingStartTime = MyTime.CurrentTimeMillis();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody : MonoBehaviour {

	/* --- Inspector --- */
	
	/* Rythem player needs to tap to. */
	public string rythem;

	/* Radius in which melody starts playing. */
    public float radius;

	/* Radius in which the melody plays at max volume. */
	public float maxVolRadius;

	/* Minimum amount of time enemy will wait before restarting. */
	public int minWaitingTime;

	/* Player. */
	public GameObject player;


	/* --- Attributes --- */

	public const int BLOCK_DURATION = 2;

	/* Audio Source. */
	private AudioSource aSrc;

	/* True if audio is currently playing. */
	private bool playing;

	/* True if enemy already waited due to player pressing skill. */
	private bool waitedAfterSkill;

	/* Time of the previous block. */
	private int previousBlockTime;

	/* Time enemy started waiting. */
	private long waitingStartTime;

	/* Default max volume. */
	private float defaultVolume;


    /* --- Methods --- */

    /// <summary>
    /// Initialization.
    /// </summary>
    void Start() {

        aSrc = gameObject.GetComponent<AudioSource>();
		playing = false;
        waitingStartTime = 0;
        previousBlockTime = 0;
		defaultVolume = aSrc.volume;
		waitedAfterSkill = false;
	}


    /// <summary>
    /// Update once per frame.
    /// </summary>
    void Update()
    {
		if (!aSrc.isPlaying)
		{
			playing = false;
		}
		
		double dist = Vector3.Distance(player.transform.position, transform.position);
        int currentTime = (int)Time.time;       

		if (dist <= radius)
		{
			aSrc.volume = defaultVolume - (float)((dist - maxVolRadius) * (defaultVolume) / (radius - maxVolRadius));

            if (currentTime % BLOCK_DURATION == 0 && !playing && currentTime - previousBlockTime > BLOCK_DURATION)
            {
				Play();
				previousBlockTime = currentTime;				
			}
		}
		else if (playing)
		{
			Stop(false);
		}
		else
		{
			aSrc.volume = 0;
		}     
    }


    /// <summary>
    /// Starts playback of this melody.
    /// </summary>
    public void Play()
	{
        playing = true;
        waitedAfterSkill = false;
        aSrc.Play();
	}


    /// <summary>
    /// Stops playback of this melody.
    /// </summary>
    /// <param name="playerSkill">True if called due to player skill.</param>
    public void Stop(bool playerSkill)
	{
		playing = false;
		aSrc.Stop();

        int currentTime = (int)Time.time;
        if (playerSkill && BLOCK_DURATION - (currentTime % BLOCK_DURATION) < minWaitingTime)
		{
			if (!waitedAfterSkill)
			{
                previousBlockTime += BLOCK_DURATION;
				waitedAfterSkill = true;
			}		
		}
		else if (!playerSkill)
		{
			waitedAfterSkill = false;
		}
	}
}

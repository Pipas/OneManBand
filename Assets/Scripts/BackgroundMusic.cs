using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

	/* --- Inspector */

	/* Array of audio clips to cycle through. */
	public AudioClip[] aClips;


	/* --- Attributes --- */

	/* Audio Source that plays the audio. */
	private AudioSource aSrc;

	/* Index of the currently playing audio. */
	private int iAudio;
	

	/* --- Methods --- */

	// Use this for initialization
	void Start () {
        iAudio = 0;
        aSrc = GetComponent<AudioSource>();
        aSrc.clip = aClips[iAudio];
        aSrc.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (!aSrc.isPlaying && iAudio < aClips.Length - 1)
		{
            Debug.Log("Swapped audio clip!");
			aSrc.clip = aClips[++iAudio];
            aSrc.Play();
		}
	}
}

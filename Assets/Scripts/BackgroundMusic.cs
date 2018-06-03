using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

	public AudioClip zeroIntro;
	public AudioClip oneWhile;
	public AudioClip twoEnd;

	private AudioSource audioSource;
	private int currAudio;
	private AudioClip[] audios;

	// Use this for initialization
	void Start () {
		audios = new[] {zeroIntro, oneWhile, twoEnd};
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = zeroIntro;
		audioSource.Play();
		currAudio = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!audioSource.isPlaying && currAudio < audios.Length - 1)
		{
			audioSource.clip = audios[++currAudio];
			audioSource.Play();
		}
	}
}

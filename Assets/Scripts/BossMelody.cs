using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMelody : Melody {

    [System.Serializable]
    public class InstList
    {
        public GameObject[] instruments;
    }

    /* --- Inspector --- */

    /* Audio Clips for stages 1...n. */
    public AudioClip[] otherClips;
	
	/* Rythems for stages 1...n. */
	public string[] otherRythems;

	/* Required instruments for stages 1...n. */
	public InstList[] otherInstruments;


	/* --- Attrs --- */

	/* Current stage. */
	private int stage;


	/* --- Methods --- */

	// Use this for initialization
	new void Start () {
		base.Start();
		stage = -1;
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	}

	// move to next stage
	public void NextStage()
	{
        if (stage >= otherRythems.Length - 1 &&
			stage >= otherInstruments.Length - 1 &&
			stage >= otherClips.Length - 1)
		{
			return;
		}

		stage++;

		rythem = otherRythems[stage];
		instruments = otherInstruments[stage].instruments;
        aSrc.clip = otherClips[stage];
	}
}

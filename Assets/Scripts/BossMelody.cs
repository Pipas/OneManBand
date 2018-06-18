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

	/* True if the boss is active. */
	private bool active;

	/* Instance. */
	private static BossMelody self;


	/* --- Methods --- */

	// Use this for initialization
	new void Start () {
		base.Start();
		stage = -1;
        active = false;
		self = this;
	}
	
	// Update is called once per frame
	new void Update () {
		if (active)
		{
			base.Update();
		}
	}

	public static void Activate()
	{
		self.active = true;
	}

    public static void Deactivate()
    {
        self.active = false;
    }

	// move to next stage
	public void NextStage()
	{
        if (stage >= otherRythems.Length - 1 ||
			stage >= otherInstruments.Length - 1 ||
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

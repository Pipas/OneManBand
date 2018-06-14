using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

    [System.Serializable]
    public class AudioList
    {
        public AudioClip[] aClips;
    }

	// start > while > end
	private abstract class State {
        protected BGM inst;

        protected int iAudio;

        public State(BGM inst)
        {
            this.inst = inst;
        }

		public abstract void update();
	}

	private class StartState: State {

		public StartState(BGM inst) : base(inst)
		{
			iAudio = 0;
            inst.bgmASrc.clip = inst.aStart[iAudio];
			inst.bgmASrc.Play();
		}

		public override void update() {
            if (!inst.bgmASrc.isPlaying)
            {
                Debug.Log("Swapped audio clip!");

                if (iAudio < inst.aStart.Length - 1)
                {
                    inst.bgmASrc.clip = inst.aStart[++iAudio];
                    inst.bgmASrc.Play();
                }
				else
				{
					inst.state = new WhileState(inst);
				}
            }
		}
	}

    private class WhileState : State
    {
		private int reps;
		private int iTheme;

        public WhileState(BGM inst) : base(inst)
        { 
            iTheme = Random.Range(0, inst.aWhile.Length);
			iAudio = Random.Range(0, inst.aWhile[iTheme].aClips.Length);
			reps = 0;
		}

        public override void update()
        {
            if (!inst.bgmASrc.isPlaying)
            {
                reps++;

				if (reps >= 3)
				{
					int oldITheme = iTheme;
					iTheme = Random.Range(0, inst.aWhile.Length);
					if (oldITheme != iTheme)
					{
						reps = 0;
                        Debug.Log("Swapped audio theme!");
					}					
				}

                Debug.Log("Swapped audio clip!");

                iAudio = Random.Range(0, inst.aWhile[iTheme].aClips.Length);
                inst.bgmASrc.clip = inst.aWhile[iTheme].aClips[iAudio];
                inst.bgmASrc.Play();
            }
        }
    }

	/* --- Inspector */

	/* Array of audio clips to cycle through. */
	public AudioClip[] aStart;

    /* Array of audio clips to cycle through. */
    public AudioList[] aWhile;

	/* 0: guitar, 1: drums. */
	public AudioClip[] aFoundInst;


	/* --- Attributes --- */

	/* Audio Source that plays the bgm. */
	private AudioSource bgmASrc;

	/* Audio Source that plays foundInst sound. */
	private AudioSource foundASrc;

	/* Self instance. */
	private static BGM self;
	
	/* Current state. */
	private State state;

	/* --- Methods --- */

	// Use this for initialization
	void Start () {
		self = this;
		AudioSource[] aSrcs = GetComponents<AudioSource>();
        bgmASrc = aSrcs[0];
		foundASrc = aSrcs[1];
        state = new StartState(this);
	}
	
	// Update is called once per frame
	void Update () {
		state.update();
	}

	// Play the instrument sound when found
	public static void FoundInst(string name)
	{
		AudioClip clip = null;
		switch (name)
		{
			case "PartyGuitar":
				clip = self.aFoundInst[1];
				break;

			default:
				break;
		}

		self.foundASrc.clip = clip;

		// curent time (rounded to seconds)
        int currentTime = (int)Time.time;

		int mod = currentTime % 4;
        if (mod == 0)
		{
            self.foundASrc.Play();
		}
		else
		{
			int diff = 4 - mod;
			self.foundASrc.PlayDelayed((currentTime + diff) - Time.time);
		}		
	}
}

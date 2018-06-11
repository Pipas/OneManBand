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
            inst.aSrc.clip = inst.aStart[iAudio];
			inst.aSrc.Play();
		}

		public override void update() {
            if (!inst.aSrc.isPlaying)
            {
                Debug.Log("Swapped audio clip!");

                if (iAudio < inst.aStart.Length - 1)
                {
                    inst.aSrc.clip = inst.aStart[++iAudio];
                    inst.aSrc.Play();
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
            if (!inst.aSrc.isPlaying)
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
                inst.aSrc.clip = inst.aWhile[iTheme].aClips[iAudio];
                inst.aSrc.Play();
            }
        }
    }

	/* --- Inspector */

	/* Array of audio clips to cycle through. */
	public AudioClip[] aStart;

    /* Array of audio clips to cycle through. */
    public AudioList[] aWhile;


	/* --- Attributes --- */

	/* Audio Source that plays the audio. */
	private AudioSource aSrc;
	
	private State state;

	/* --- Methods --- */

	// Use this for initialization
	void Start () {
        aSrc = GetComponent<AudioSource>();
        state = new StartState(this);
	}
	
	// Update is called once per frame
	void Update () {
		state.update();
	}
}

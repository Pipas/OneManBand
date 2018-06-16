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

        public void updateVolume(float volume)
        {
            inst.bgmASrc.volume = volume;
        }
	}

	private class StartState: State {

		public StartState(BGM inst) : base(inst)
		{
			iAudio = 0;
            inst.bgmASrc.clip = inst.aStart[iAudio];
            inst.bgmASrc.volume = StaticSettings.volumeBGM;
			inst.bgmASrc.Play();
		}

		public override void update() {
            if (!inst.bgmASrc.isPlaying)
            {
                if (iAudio < inst.aStart.Length - 1)
                {
                    inst.bgmASrc.clip = inst.aStart[++iAudio];
                    inst.bgmASrc.volume = StaticSettings.volumeBGM;
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
					}					
				}

                iAudio = Random.Range(0, inst.aWhile[iTheme].aClips.Length);
                inst.bgmASrc.clip = inst.aWhile[iTheme].aClips[iAudio];

                inst.bgmASrc.volume = StaticSettings.volumeBGM;
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

    public void updateBGMVolume(float volume)
    {
        state.updateVolume(volume);
    }

	// Play the instrument sound when found
	public static void FoundInst(string name)
	{
		AudioClip clip = null;

		switch (name)
		{
			case "PartyTambor":
				clip = self.aFoundInst[0];
				break;
            case "PartyGuitar":
                clip = self.aFoundInst[1];
                break;

			default:
				break;
		}

		self.foundASrc.clip = clip;
		self.foundASrc.Play();
	}
}

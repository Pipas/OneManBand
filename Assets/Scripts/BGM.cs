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
		protected bool already;
		protected bool fadeout;
        protected bool fadein;

        public State(BGM inst)
        {
            this.inst = inst;
			already = false;
			fadeout = false;
			fadein = false;
        }

		public virtual void update()
		{
            if (inst.isAnyTriggerableOn())
			{
				fadeout = true;
				fadein = false;
			}
			else
			{
                fadeout = false;
				fadein = true;
			}
			
			if (fadeout)
			{
                if (inst.bgmASrc.volume > inst.minBGMVolume)
                {
                    inst.bgmASrc.volume -= (Time.deltaTime * (inst.DEFAULT_VOLUME - inst.minBGMVolume) / 1.0f);
                }
			}
			else if (fadein)
			{
                if (inst.bgmASrc.volume < inst.DEFAULT_VOLUME)
                {
                    inst.bgmASrc.volume += (Time.deltaTime * (inst.DEFAULT_VOLUME - inst.minBGMVolume) / 1.0f);
                }
			}
		}
	}

	private class BossState: State {

		private AudioSource bossASrc;
		private static BossState self;

		public BossState(BGM inst) : base(inst)
		{
			fadeout = true;
			fadein = true;
			bossASrc = inst.foundASrc;
			iAudio = 0;
			bossASrc.loop = false;
            bossASrc.volume = 0;
			bossASrc.clip = inst.aBoss[iAudio];
			bossASrc.Play();
			self = this;
		}

		public override void update()
		{
            // fade out normal bgm
			if (fadeout)
            {
                if (inst.bgmASrc.volume > 0)
                {
                    inst.bgmASrc.volume -= (Time.deltaTime * inst.DEFAULT_VOLUME / (inst.bossBGMFadeDuration + 0.1f));
                }
				else
				{
					fadeout = false;
					inst.bgmASrc.Stop();
				}
            }

			// fade in boss bgm
            if (fadein)
            {
                if (bossASrc.volume < inst.DEFAULT_VOLUME)
                {
                    bossASrc.volume += (Time.deltaTime * inst.DEFAULT_VOLUME / (inst.bossBGMFadeDuration + 0.1f));
                }
                else
                {
                    fadein = false;
                }
            }
			
			// next stage
			if (!bossASrc.isPlaying && iAudio <= inst.aBoss.Length - 1)
            {
                if (iAudio == 0)
				{
					BossMelody.Activate();
					iAudio++;
				}

                bossASrc.clip = inst.aBoss[iAudio];

                bossASrc.Play();
				bossASrc.loop = true;
            }
		}

		public static void NextClip()
		{
			if (self.iAudio <= 0)
			{
				return;
			}
			
			self.bossASrc.loop = false;
            if (self.iAudio < self.inst.aBoss.Length - 1)
			{
                self.iAudio++;

				// instantly swap to victory clip if boss is defeated!
				if (self.iAudio == self.inst.aBoss.Length - 1)
				{
                    self.bossASrc.clip = self.inst.aBoss[self.iAudio];

                    self.bossASrc.Play();
                    self.bossASrc.loop = true;
				}
			}
		}
	}

	private class GameOverState: State {
		private float duration;
		private float startVolume;
        public GameOverState(BGM inst, float duration) : base(inst)
        {
			this.duration = duration;
			inst.bgmASrc.loop = true;
			startVolume = inst.bgmASrc.volume;
        }

        public override void update()
        {
            if (inst.bgmASrc.volume > 0)
			{
                inst.bgmASrc.volume -= (Time.deltaTime * startVolume / (duration + 0.1f));
			}
			else
			{
				inst.bgmASrc.Stop();
			}
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
            base.update();

            if (!inst.bgmASrc.isPlaying)
            {
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

            // randomly play theme of instruments in party
            if ((int)Time.timeSinceLevelLoad % 8 == 0)
            {
                if (!already)
                {
                    inst.playInstTheme();
                    already = true;
                }
            }
            else
            {
                already = false;
            }

            // swap to boss theme
            if (inst.playBossBGM && (int)Time.timeSinceLevelLoad % 4 == 0)
            {
                inst.state = new BossState(inst);
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
            base.update();

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

                inst.bgmASrc.Play();
            }

			// randomly play theme of instruments in party
			if ((int)Time.timeSinceLevelLoad % 8 == 0)
			{
				if (!already)
				{
					inst.playInstTheme();
					already = true;
				}				
			}
			else
			{
				already = false;
			}

            // swap to boss theme
            if (inst.playBossBGM && (int)Time.timeSinceLevelLoad % 4 == 0)
			{
				inst.state = new BossState(inst);
			}
        }
    }

	/* --- Inspector */

	/* Volume of bgm while near triggerables. */
	public float minBGMVolume;

	/* Duration of fade in of boss bgm. */
	public float bossBGMFadeDuration;

	/* Chance of playing instruments theme. */
	public int instThemeChance;

	/* Array of audio clips to cycle through. */
	public AudioClip[] aStart;

    /* Array of audio clips to cycle through. */
    public AudioList[] aWhile;

	/* Array of audio clips to cycly through during boss fights. */
	public AudioClip[] aBoss;

	/* 0: drums, 1: guitar, 2: piano. */
	public AudioClip[] aFoundInst;

    /* 0: drums, 1: guitar, 2: piano. */
    public AudioList[] aInstTheme;

	/* Clip for when finding sheets. */
	public AudioClip aFoundSheet;


	/* --- Attributes --- */

	/* Audio Source that plays the bgm. */
	private AudioSource bgmASrc;

	/* Audio Source that plays when something is found. */
	private AudioSource foundASrc;

    /* Audio Source that plays drums theme. */
    private AudioSource drumsASrc;

	/* Audio Source that plays guitar theme. */
	private AudioSource guitarASrc;

    /* Audio Source that plays guitar theme. */
    private AudioSource pianoASrc;

	/* Self instance. */
	private static BGM self;

	/* Default bgm volume. */
	private float DEFAULT_VOLUME;
	
	/* Current state. */
	private State state;

	/* True if drums was already found previously, false otherwise. */
	private bool alreadyFoundDrums;

    /* True if guitar was already found previously, false otherwise. */
    private bool alreadyFoundGuitar;

    /* True if piano was already found previously, false otherwise. */
    private bool alreadyFoundPiano;

	/* True if boss bgm should be played now. */
	private bool playBossBGM;


	/* --- Methods --- */

	// Use this for initialization
	void Start () {
		self = this;
		AudioSource[] aSrcs = GetComponents<AudioSource>();
        bgmASrc = aSrcs[0];
		foundASrc = aSrcs[1];
		drumsASrc = aSrcs[2];
        guitarASrc = aSrcs[3];
        pianoASrc = aSrcs[4];
        alreadyFoundDrums = false;
        alreadyFoundGuitar = false;
		alreadyFoundPiano = false;
		DEFAULT_VOLUME = StaticSettings.volumeBGM;
		playBossBGM = false;
        state = new StartState(this);
	}
	
	// Update is called once per frame
	void Update () {
		state.update();
	}

    public void updateBGMVolume(float volume)
    {
		// prevent 0
		volume += 0.01f;
		bgmASrc.volume = volume;
		if (state is BossState)
		{
			foundASrc.volume = volume;
		}
		minBGMVolume = minBGMVolume * (volume / DEFAULT_VOLUME);
		DEFAULT_VOLUME = volume;
    }

	private void playInstTheme()
	{
		// make sure playing this wont interfere with gameplay
		if (foundASrc.isPlaying || isAnyTriggerableOn())
		{
			return;
		}

		bool drums = false;
		bool guitar = false;
        bool piano = false;
		
		foreach (GameObject inst in Movement.party)
		{
			if (inst.name == "PartyTambor")
			{
				drums = true;
			} else if (inst.name == "PartyGuitar")
			{
				guitar = true;
            } else if (inst.name == "PartyPiano")
            {
                piano = true;
            }
		}

		if (!drumsASrc.isPlaying && drums && Random.Range(0, 100) >= (100 - instThemeChance))
		{
			drumsASrc.clip = aInstTheme[0].aClips[Random.Range(0, aInstTheme[0].aClips.Length)];
			drumsASrc.Play();
		}
        
		if (!guitarASrc.isPlaying && guitar && Random.Range(0, 100) >= (100 - instThemeChance))
        {
            guitarASrc.clip = aInstTheme[1].aClips[Random.Range(0, aInstTheme[1].aClips.Length)];
			guitarASrc.Play();
        }

        if (!pianoASrc.isPlaying && piano && Random.Range(0, 100) >= (100 - instThemeChance))
        {
            pianoASrc.clip = aInstTheme[2].aClips[Random.Range(0, aInstTheme[2].aClips.Length)];
            pianoASrc.Play();
        }
	}

	// stops currently playing themes
	public static void StopInstThemes()
	{
		self.drumsASrc.Stop();
		self.guitarASrc.Stop();
        self.pianoASrc.Stop();
	}

	// fade out the bgm and fade in gameover theme
	public static void GameOver(float duration)
	{
		self.state = new GameOverState(self, duration);
	}

	// plays the boss bgm
	public static void PlayBossBGM()
	{
		self.playBossBGM = true;
	}

	// plays the bgm for the next boss stage
	public static void PlayBossNextBGM()
	{
        if (self.state is BossState)
		{
			BossState.NextClip();
		}
	}

	// Play the instrument sound when found
	public static void FoundInst(string name)
	{
		if (self.state is BossState)
		{
			return;
		}
		
		AudioClip clip = null;

		switch (name)
		{
			case "PartyTambor":
				if (!self.alreadyFoundDrums)
				{
					clip = self.aFoundInst[0];
					self.alreadyFoundDrums = true;
				}
				break;

            case "PartyGuitar":
				if (!self.alreadyFoundGuitar)
				{
					clip = self.aFoundInst[1];
                    self.alreadyFoundGuitar = true;
				}                
                break;

			case "PartyPiano":
                if (!self.alreadyFoundPiano)
                {
                    clip = self.aFoundInst[2];
                    self.alreadyFoundPiano = true;
                }
                break;

			default:
				break;
		}

		self.foundASrc.clip = clip;
		if (clip != null)
		{
			self.foundASrc.Play();
		}
	}

    // Play the sheet sound when found
    public static void FoundSheet()
    {
        self.foundASrc.clip = self.aFoundSheet;
        self.foundASrc.Play();
    }

	// true if there's at least one triggerable active at the moment
	private bool isAnyTriggerableOn()
	{
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] spotlights = GameObject.FindGameObjectsWithTag("Spotlight");

        GameObject[] triggerables = new GameObject[enemies.Length + spotlights.Length];
        System.Array.Copy(enemies, triggerables, enemies.Length);
        System.Array.Copy(spotlights, 0, triggerables, enemies.Length, spotlights.Length);

        foreach (GameObject obj in triggerables)
        {
            if (!obj.GetComponent<Melody>().IsStopped() || obj.tag == "Spotlight" && obj.GetComponent<Spotlight>().hasPlayer)
            {
                return true;
            }

        }

		return false;
	}
}

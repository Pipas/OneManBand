using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody : MonoBehaviour {

    private abstract class State
    {
        protected Melody inst;
        protected double dist;
        protected int currentTime;

        public State(Melody inst)
        {
            this.inst = inst;
            dist = Vector3.Distance(inst.player.transform.position, inst.transform.position);
            currentTime = (int)Time.time;
        }

        public virtual void update()
        {
            dist = Vector3.Distance(inst.player.transform.position, inst.transform.position);
            currentTime = (int)Time.time;
        }
    }

    private class PlayingState : State
    {

        public PlayingState(Melody inst) : base(inst)
        {
            inst.aSrc.Play();
        }

        public override void update()
        {
            base.update();

            // if player in range
            if (dist <= inst.radius)
            {
                // update volume according to range
                inst.aSrc.volume = inst.defaultVolume - (float)((dist - inst.maxVolRadius) * (inst.defaultVolume) / (inst.radius - inst.maxVolRadius));

                // if melody is over
                if (!inst.aSrc.isPlaying)
                {
                    // wait for player window
                    inst.state = new WaitingState(inst);
                }
            }
            else
            {
                // stop playing
                inst.state = new StopState(inst);
            }
        }
    }

    private class StopState : State
    {

        public StopState(Melody inst) : base(inst)
        {
            inst.aSrc.Stop();
            inst.aSrc.volume = 0;
        }

        public override void update()
        {
            base.update();

            // if player in range
            if (dist <= inst.radius)
            {
                // wait correct timing
                if (currentTime % 4 == 0)
                {
                    // start playing
                    inst.state = new PlayingState(inst);
                }
            }
        }
    }

    private class WaitingState : State
    {
        private int startTime;

        public WaitingState(Melody inst) : base(inst)
        {
            inst.aSrc.Stop();
            startTime = currentTime;
        }

        public override void update()
        {
            base.update();

            Debug.Log(dist);
            // if player in range
            if (dist <= inst.radius)
            {
                // wait correct timing (min 2s waiting)
                if (currentTime % 4 == 0 && currentTime - startTime > 2)
                {
                    // start playing
                    inst.state = new PlayingState(inst);
                }
            }
            else
            {
                // no need to wait if player not in range
                inst.state = new StopState(inst);
            }
        }
    }

    /* --- Inspector --- */

    /* Menu UI. */
    public InGameMenu menu;
	
	/* Rythem player needs to tap to. */
	public string rythem;

	/* Radius in which melody starts playing. */
    public float radius;

	/* Radius in which the melody plays at max volume. */
	public float maxVolRadius;

	/* Player. */
	public GameObject player;


	/* --- Attributes --- */

	/* Audio Source. */
	private AudioSource aSrc;

	/* Default max volume. */
	private float defaultVolume;

    /* Current state. */
    private State state;

    /* True if the game is paused, false otherwise. */
    public bool paused;


    /* --- Methods --- */

    /// <summary>
    /// Initialization.
    /// </summary>
    void Start()
    {
        aSrc = gameObject.GetComponent<AudioSource>();
		defaultVolume = aSrc.volume;
        paused = false;
        state = new StopState(this);
	}


    /// <summary>
    /// Update once per frame.
    /// </summary>
    void Update()
    {
        // if game is paused
        if (menu.isGameStopped())
        {
            // pause audio playback
            if (aSrc.isPlaying)
            {
                paused = true;
                aSrc.Pause();
            }
        }
        else
        {
            // resume audio playback
            if (paused)
            {
                paused = false;
                aSrc.UnPause();
            }

            // update
            state.update();
        }        
    }


    /// <summary>
    /// Called when the player presses a skill and enemy should wait.
    /// </summary>
    public void Wait()
    {
        state = new WaitingState(this);
    }

    public bool IsStopped()
    {
        return (state is StopState);
    }
}

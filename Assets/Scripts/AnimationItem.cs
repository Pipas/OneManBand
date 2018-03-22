using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationItem
{
	private Vector3 vector;
	private float speed;
	private bool savePosition;
	private bool triggerParty;
	
	public AnimationItem(Vector3 v, float s, bool sP, bool tP)
	{
		vector = v;
		speed = s;
		savePosition = sP;
		triggerParty = tP;
	}

	public Vector3 GetVector()
	{
		return vector;
	}

	public float GetSpeed()
	{
		return speed;
	}

	public bool SavePosition()
	{
		return savePosition;
	}

	public bool TriggerParty()
	{
		return triggerParty;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationItem
{
	private Vector3 vector;
	private bool checkForward;
	private bool triggerParty;
	private bool savePosition;

	public AnimationItem(Vector3 v, bool sP, bool tP)
	{
		vector = v;
		savePosition = sP;
		triggerParty = tP;
	}

	public Vector3 GetVector()
	{
		return vector;
	}

	public bool TriggerParty()
	{
		return triggerParty;
	}

	public bool SavePosition()
	{
		return savePosition;
	}
}

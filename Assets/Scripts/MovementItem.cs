using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementItem
{
	private Vector3 vector;
	private bool checkForward;
	private bool triggerParty;
	private bool savePosition;

	public MovementItem(Vector3 v, bool cF, bool tP, bool sP)
	{
		vector = v;
		checkForward = cF;
		triggerParty = tP;
		savePosition = sP;
	}

	public MovementItem(Vector3 v)
	{
		vector = v;
		checkForward = true;
		triggerParty = true;
		savePosition = true;
	}

	public Vector3 GetVector()
	{
		return vector;
	}

	public bool CheckForward()
	{
		return checkForward;
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

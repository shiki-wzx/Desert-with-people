using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EventType
{
	AchivementAccomplished = 0,
	LetterReceived = 1
}

public enum GameAction
{
	AchivementAccomplish = 0,
}
public class GameEvent:MonoBehaviour
{
	virtual public void OnNotify() { }
	virtual public void InOutAction(GameAction action) { }
}

public class TaskEvent : GameEvent
{
	public override void OnNotify()
	{
		FindObjectOfType<GameFlowCtrler>()._instance.currentTaskIndex++;
		//broadcast next task
	}
	public override void InOutAction(GameAction action)
	{
		//send letters
	}
}

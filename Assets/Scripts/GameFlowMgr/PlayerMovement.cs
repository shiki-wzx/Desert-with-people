using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerActions;
/// <summary>
/// 当要执行move的时候调用这个接口
/// </summary>
public class PlayerMovement : SingletonMono<PlayerMovement>
{
	private void MapBlkClickHandle(BlkRtInfo info)
	{
		if (currAction == ActionType.None)
			return;
		if (info.HasActionQueued)
			return;

		if (currAction == ActionType.DesertHandle)
		{
			info.SandControl();
			//todo : block recolor audiofx
		}
		else
		{
			info.Planting();
			//todo : block recolor audiofx
		}

		currAction = ActionType.None;
	}

	//protected override void OnInstanceAwake()
	//{}

	private void OnEnable() => CameraControl.Instance.ClickCallback += MapBlkClickHandle;
	private void OnDisable() => CameraControl.Instance.ClickCallback -= MapBlkClickHandle;

	private ActionType currAction = ActionType.None;

	public void Move(ActionType action)
	{
		int cost = 1;
		switch (action)
		{
			case ActionType.DesertHandle:
				{
					if (currAction == ActionType.None)
					{
						currAction = ActionType.DesertHandle;
						cost = 10;
						//todo : switch mat
						GameFlowCtrler.Instance.labourForce -= (int)(cost * GameFlowCtrler.Instance.labourForceCostCoef);
					}
					break;
				}
			case ActionType.Plant:
				{
					if (currAction == ActionType.None)
					{
						currAction = ActionType.Plant;
						cost = 30;
						GameFlowCtrler.Instance.labourForce -= (int)(cost * GameFlowCtrler.Instance.labourForceCostCoef);
					}
					break;
				}
		}
	}
}

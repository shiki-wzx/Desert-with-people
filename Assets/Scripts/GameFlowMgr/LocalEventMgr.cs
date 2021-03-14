using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEventMgr : SingletonMono<LocalEventMgr>
{
	public void ExcuteEvent(Event thisEvent, out bool skipDesertify)
	{
		GameFlowCtrler ctrler = FindObjectOfType<GameFlowCtrler>();
		ctrler.labourForceMaximun = (int)(ctrler.labourForceMaximun + thisEvent.riseLabourForceMaximunBy);
		ctrler.labourForce += (int)thisEvent.labourForceIncrease;
		ctrler.labourForceCostCoef = thisEvent.labourForceCostMultiplier;
		skipDesertify = !thisEvent.desertify;
		if (thisEvent.blockRandomDegredate == true)
		{
			if (MapMgr.Instance.CountBlk(BlockType.ZhiWu) >= 2)
				MapMgr.Instance.RandomDegrade(2, BlockType.CaoDi, BlockType.ZhiWu);
			if (MapMgr.Instance.CountBlk(BlockType.ZhiWu) == 1)
			{
				MapMgr.Instance.RandomDegrade(1, BlockType.CaoDi, BlockType.ZhiWu);
			}
			MapMgr.Instance.RandomDegrade(2, BlockType.PingDi, BlockType.ShaQiu, BlockType.ShaDi);
		}
	}
}


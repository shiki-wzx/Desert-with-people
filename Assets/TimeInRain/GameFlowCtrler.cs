using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class StateInfo
{
	public BlockType type;
	public int num;
	public StateInfo(BlockType _type) { type = _type; }
}
public class GameFlowCtrler : MonoBehaviour
{
	// Start is called before the first frame update
	public GameFlowCtrler _instance { get; private set; }
	public int currentTaskIndex = 0;
	public enum Season
	{
		Spring = 0,
		Summer,
		Fall,
		Winter
	};
	/*
     * ShaQiu,
     * ShaDi,
     * PingDi,
     * CaoDi,
     * YouMiao,
     * ZhiWu,
     * ZhiShaZhan
     */
	public class currentCompletation
	{
		public StateInfo ShaQiu = new StateInfo(BlockType.ShaQiu);
		public StateInfo ShaDi = new StateInfo(BlockType.ShaDi);
		public StateInfo PingDi = new StateInfo(BlockType.PingDi);
		public StateInfo CaoDi = new StateInfo(BlockType.CaoDi);
		public StateInfo YouMiao = new StateInfo(BlockType.YouMiao);
		public StateInfo ZhiWu = new StateInfo(BlockType.ZhiWu);
		public StateInfo ZhiShaZhan = new StateInfo(BlockType.ZhiShaZhan);
		public StateInfo test_none = new StateInfo(BlockType.None);
	}
	public int roundCount = 0;//also combined with season
	public Season currentSeason;
	[ShowInInspector]
	public currentCompletation StateInformation=new currentCompletation();


	protected void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void RefreshStateList()
	{
		//接口
		//你用自己的也可以
	}

	public void NewRound()
	{
		roundCount++;
		RefreshStateList();
		SeasonAdjust(roundCount);
		FindObjectOfType<PlayerMovement>().NewRound((int)(roundCount*1.3+1.7));//比例系数随意更改，控制游戏难度
	}

	void SeasonAdjust(int round)
	{
		currentSeason = (Season)Mathf.Floor(round / 2);
	}
	
}

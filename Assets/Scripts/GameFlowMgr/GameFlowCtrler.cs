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
public enum Season
{
	Spring = 0,
	Summer,
	Fall,
};
public class Round
{
	public int roundCount;
	public Season currentSeason;
	public int noDegredationRoundCount;
	public static Round operator ++(Round round)
	{
		if (GameFlowCtrler.Instance.mapUpdInfo.DegradationTriggered == false)
		{
			round.noDegredationRoundCount++;
		}
		else round.noDegredationRoundCount = 0;
		round.roundCount++;
		return round;
	}
	public Round()
	{
		roundCount = 0;
		noDegredationRoundCount = 0;
		currentSeason = Season.Spring;
	}
}
public class GameFlowCtrler : SingletonMono<GameFlowCtrler>
{
	// Start is called before the first frame update
	#region DisplayLayer
	public UIManager m_UIMgr;
	public CameraControl m_camCtrl;
	#endregion
	[ShowInInspector]
	public EventSettings eventSettings;
	public List<Event> eventPool;
	public List<Event> InRoundEventPool;
	public List<bool> accomplishedAchivement;
	[ReadOnly]
	public MapStatInfo mapStatus;
	[HideInInspector]
	public LetterInfo letterInfo;
	[ReadOnly]
	public MapUpdInfo mapUpdInfo;
	[ReadOnly]
	public LocalEventMgr localEventMgr;
	[ReadOnly]
	[SerializeField]
	private EventSystem eventSystem;
	[HideInInspector]
	bool skipDesertify = false;
	bool thirdLetterTriggered = false;
	//public class currentCompletation
	//{
	//	public StateInfo ShaQiu = new StateInfo(BlockType.ShaQiu);
	//	public StateInfo ShaDi = new StateInfo(BlockType.ShaDi);
	//	public StateInfo PingDi = new StateInfo(BlockType.PingDi);
	//	public StateInfo CaoDi = new StateInfo(BlockType.CaoDi);
	//	public StateInfo YouMiao = new StateInfo(BlockType.YouMiao);
	//	public StateInfo ZhiWu = new StateInfo(BlockType.ZhiWu);
	//	public StateInfo ZhiShaZhan = new StateInfo(BlockType.ZhiShaZhan);
	//	public StateInfo test_none = new StateInfo(BlockType.None);
	//}
	public Round round;//also combined with season
	[Header("GameRoundInfo")]
	[SerializeField]
	public int labourForceMaximun = 10;
	[SerializeField]
	public int labourForce = 10;
	public float labourForceCostCoef = 1.0f;
	[SerializeField]
	public Season currentSeason;
	//public currentCompletation StateInformation = new currentCompletation();
	[Button]
	public void ReadInInfo()
	{
		eventSystem = GetComponent<EventSystem>();
		localEventMgr = FindObjectOfType<LocalEventMgr>();
		RegisterEvents(eventSettings);
		for (var i = 0; i < eventSystem.achivementsSettings.achivements.Count; i++)
			accomplishedAchivement.Add(false);
		round = new Round();
	}

	protected override void OnInstanceAwake()
	{
		eventPool.Clear();
		InRoundEventPool.Clear();
		accomplishedAchivement.Clear();
		eventSystem = GetComponent<EventSystem>();
		m_UIMgr = UIManager.Instance;
	}
	protected void OnEnable()
	{
		RegisterEvents(eventSettings);
		for (var i = 0; i < eventSystem.achivements.Count; i++)
			accomplishedAchivement.Add(false);
		round = new Round();
	}
	private void Start()
	{
		m_UIMgr.GetNewMail();
		m_camCtrl = FindObjectOfType<CameraControl>();
	}
	void RegisterEvents(EventSettings eventSettings)
	{
		foreach (var thisEvent in eventSettings.settings)
		{
			eventPool.Add(thisEvent);
			if (thisEvent.property == LocalEventProperty.InRound)
			{
				for (int i = 0; i < thisEvent.probabilityWeight; i++)
					InRoundEventPool.Add(thisEvent);
			}
		}
	}
	/// <summary>
	/// Use this func to start new turn
	/// </summary>
	[Button]
	public void NewRound()
	{
		//轮次流程：在当前轮次结束前执行当前轮次的事件,然后更新地块，然后测试成就，然后调整季节，然后轮次+1（当前轮次结束），劳动力回复，执行下一轮的随机事件
		ExcuteEvent(round.roundCount, false);
		UpdateBlkInfo();
		AchivementTest(AchivementUpd());
		SeasonAdjust(round.roundCount);
		//on last round end
		round++;
		LabourForceRecover();
		// on new round begin
		ExcuteEvent(round.roundCount, true);
	}
	AchivementReq AchivementUpd()
	{
		AchivementReq achi = new AchivementReq();
		achi.AnyGreenBlkWhoseAdjAllGreen = mapStatus.AnyGreenBlkWhoseAdjAllGreen;
		achi.AnyNotDesertBlkWhoseAdjDesertEq0 = mapStatus.AnyNotDesertBlkWhoseAdjDesertEq0;
		achi.AnyNotDesertBlkWhoseAdjDesertLt4 = mapStatus.AnyNotDesertBlkWhoseAdjDesertLt4;
		achi.letterCount = letterInfo.currentLetterCount;
		achi.greenBlockReach = MapMgr.Instance.CountBlk(BlockType.CaoDi, BlockType.YouMiao, BlockType.ZhiWu);//这里存疑，治沙站算不算绿色地
		achi.noDegredationRoundCount = round.noDegredationRoundCount;
		achi.SeedingTriggered = mapUpdInfo.SeedingTriggered;
		return achi;
	}
	void LabourForceRecover()
	{
		labourForce = labourForceMaximun;
	}
	void UpdateBlkInfo()
	{
		mapUpdInfo = MapMgr.Instance.PropagateGreenValue(skipDesertify);
		mapStatus = MapMgr.Instance.GetStatus();
		skipDesertify = false;
	}
	void SeasonAdjust(int round)
	{
		currentSeason = (Season)Mathf.Floor(round / 4);
	}
	void ExcuteEvent(int roundCount, bool inRound)
	{
		if (!inRound)
		{
			//先检索执行特定事件
			foreach (var thisEvent in eventPool)
			{
				if (thisEvent.property == LocalEventProperty.OnRoundEnd)
				{
					localEventMgr.ExcuteEvent(thisEvent, out skipDesertify);
					continue;
				}
				//roundEndEvent
				if (roundCount == thisEvent.specificRound && !thisEvent.isRandomEvent)
				{
					localEventMgr.ExcuteEvent(thisEvent, out skipDesertify);
					continue;
				}
			}
		}
		else
		{
			//回合开始，执行随机事件
			LocalEventMgr.Instance.ExcuteEvent(InRoundEventPool[Random.Range(0, 12)], out skipDesertify);
		}
		//第三封信的回合，特判
		if (letterInfo.currentLetterCount == 3 && !thirdLetterTriggered)
		{
			LocalEventMgr.Instance.ExcuteEvent(eventPool[4], out skipDesertify);
			thirdLetterTriggered = true;
		}
	}
	void AchivementTest(AchivementReq achi)
	{
		if (achi.AnyNotDesertBlkWhoseAdjDesertLt4 && !accomplishedAchivement[0])
		{
			accomplishedAchivement[0] = true;
		}
		if (achi.AnyNotDesertBlkWhoseAdjDesertEq0 && !accomplishedAchivement[1])
		{
			accomplishedAchivement[1] = true;
		}
		if (achi.AnyGreenBlkWhoseAdjAllGreen && !accomplishedAchivement[2])
		{
			accomplishedAchivement[2] = true;
		}
		if (achi.SeedingTriggered && !accomplishedAchivement[3])
		{
			accomplishedAchivement[3] = true;
		}
		for (int i = 0; i < EventSystem.Instance.achivements.Count; i++)
		{
			AchivementInfo achivement = EventSystem.Instance.achivements[i];
			if (accomplishedAchivement[achivement.index] == false)
			{
				if (achi.greenBlockReach >= achivement.requirement.greenBlockReach &&
					achi.letterCount >= achivement.requirement.letterCount &&
					achi.noDegredationRoundCount >= achivement.requirement.noDegredationRoundCount
					)
				{
					accomplishedAchivement[i] = true;
				}

			}
		}
	}

	private void Update()
	{
		if (CheckNewRound())
		{
			NewRound();
		}
	}

	bool CheckNewRound()
	{
		return labourForce <= 0;
	}
}

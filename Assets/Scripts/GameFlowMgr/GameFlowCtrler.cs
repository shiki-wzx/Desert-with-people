using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
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
	public bool skipDesertify = false;
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
	public int labourForceMaximun = 100;
	[SerializeField]
	public int labourForce = 50;
	public float labourForceCostCoef = 1.0f;
	[SerializeField]
	public Season currentSeason;
	public List<bool> taskAccomplished;
	//public currentCompletation StateInformation = new currentCompletation();
	public Dictionary<PlayerActions.ActionType, int> mp=new Dictionary<PlayerActions.ActionType, int>();
	[Button]
	public void ReadInInfo()
	{
		eventSystem = GetComponent<EventSystem>();
		localEventMgr = FindObjectOfType<LocalEventMgr>();
		RegisterEvents(eventSettings);
		for (var i = 0; i < eventSystem.achivementsSettings.achivements.Count; i++)
			accomplishedAchivement.Add(false);
		for (var i = 0; i < eventSystem.taskEventSettings.tasks.Count; i++)
			taskAccomplished.Add(false);
		round = new Round();
	}

	protected override void OnInstanceAwake()
	{
		mp.Add(PlayerActions.ActionType.DesertHandle, 50);
		mp.Add(PlayerActions.ActionType.Plant, 150);
		eventPool.Clear();
		InRoundEventPool.Clear();
		accomplishedAchivement.Clear();
		eventSystem = GetComponent<EventSystem>();
		m_UIMgr = UIManager.Instance;
		for (var i = 0; i < eventSystem.achivementsSettings.achivements.Count; i++)
			accomplishedAchivement.Add(false);
		for (var i = 0; i < eventSystem.taskEventSettings.tasks.Count; i++)
			taskAccomplished.Add(false);
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
		var a = UIManager.Instance;
		m_UIMgr.GetNewMail();
		a.ChangePlantNeedNum((int)(mp[PlayerActions.ActionType.Plant] * labourForceCostCoef));
		a.ChangeControlNeedNum((int)(mp[PlayerActions.ActionType.DesertHandle] * labourForceCostCoef));
		UIManager.Instance.ShowTaskMessage(0);

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
	[Button]
	public void NewRound()
	{
		//轮次流程：在当前轮次结束前执行当前轮次的事件,然后更新地块，然后测试成就，然后调整季节，然后轮次+1（当前轮次结束），劳动力回复，执行下一轮的随机事件
		int eventIndex = ExcuteEvent(round.roundCount, false);
		UpdateBlkInfo();
		int achiId=
		AchivementTest(AchivementUpd());
		TaskTest(TaskUpd());
		SeasonAdjust(round.roundCount);
		int year = (round.roundCount / 12);
		Debug.Log(year);
		//on last round end
		UIUpd(eventIndex,achiId);
		round++;//=================================================================
		LabourForceMaximunIncrease(round.roundCount);
		LabourForceRecover();
		if(MapMgr.Instance.CountBlk(BlockType.ShaQiu, BlockType.ShaDi) > 27)
			localEventMgr.ExcuteEvent(eventPool[2], out _);
		// on new round begin
		ExcuteEvent(round.roundCount, true);
	}
	public void UIUpd(int eventIndex,int achievementIndex)
	{
		var a = UIManager.Instance;

		UIManager.Instance.changeCalendar((round.roundCount / 12)+1, (int)currentSeason);
		a.ShowEndTurnWindow(eventPool[eventIndex]);
		a.ShowAchievementWindow(achievementIndex);
		int accomplishedTaskCount = 0;
		foreach (var i in taskAccomplished)
		{	if(i)
            {
				accomplishedTaskCount++;
            }
		}


		a.ProgressBarUpd(accomplishedTaskCount);
		UIManager.Instance.GUIOnNewEventRecieved();
		Debug.Log("acTask" + accomplishedTaskCount);
		//under fix
	    UIManager.Instance.ShowTaskMessage(accomplishedTaskCount);
		
		//todo accordingto the return value show
	}

	//todo tasktest:if reached    taskaccomplished [index] set true    call audio
	public void LabourForceMaximunIncrease(int roundcount)
	{
		if( roundcount>0&&roundcount%12==0)
        {
			labourForceMaximun += 50;
			Debug.Log("new year");
        }
	}
	TaskReq TaskUpd()
    {
		TaskReq taskreq = new TaskReq();
		taskreq.GreenBlockReach = MapMgr.Instance.CountBlk(BlockType.CaoDi, BlockType.YouMiao, BlockType.ZhiWu);
		taskreq.NotDesertReach = MapMgr.Instance.CountBlk(BlockType.CaoDi, BlockType.YouMiao, BlockType.ZhiWu, BlockType.PingDi,BlockType.ZhiShaZhan);
		return taskreq;
    }
	int TaskTest(TaskReq taskreq)
    {
		int index = 0;
		for(int i=0;i<EventSystem.Instance.tasks.Count;i++)
        {
			TasksInfo task = EventSystem.Instance.tasks[i];
			if(taskAccomplished[task.taskIndex]==false)
            {
				if(taskreq.GreenBlockReach>=task.targetGreen&&taskreq.NotDesertReach>=task.targetNotDesert)
                {
					taskAccomplished[i] =true;
					UIManager.Instance.ShowSendEmail();
					UIManager.Instance.GetNewMail();
					index = i;
                }
            }
        }
		return index;
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
		currentSeason = (Season)Mathf.Floor((round / 4) % 3);
	}
	int ExcuteEvent(int roundCount, bool inRound)
	{
		int eventIndex = -1;
		if (!inRound)
		{
			//先检索执行特定事件
			foreach (var thisEvent in eventPool)
			{
				if (thisEvent.property == LocalEventProperty.OnRoundEnd)
				{
					localEventMgr.ExcuteEvent(thisEvent, out skipDesertify);
					eventIndex = thisEvent.index;
					continue;
				}
				//roundEndEvent
				if (roundCount == thisEvent.specificRound && !thisEvent.isRandomEvent)
				{
					localEventMgr.ExcuteEvent(thisEvent, out skipDesertify);
					eventIndex = thisEvent.index;
					continue;
				}
			}
		}
		else
		{
			//回合开始，执行随机事件
			eventIndex = Random.Range(0, 12);
			LocalEventMgr.Instance.ExcuteEvent(InRoundEventPool[eventIndex], out skipDesertify);
		}
		//第三封信的回合，特判
		if (letterInfo.currentLetterCount == 3 && !thirdLetterTriggered)
		{
			eventIndex = 4;
			LocalEventMgr.Instance.ExcuteEvent(eventPool[4], out skipDesertify);
			thirdLetterTriggered = true;
		}
		return eventIndex;
	}
	int AchivementTest(AchivementReq achi)
	{
		int index = 0;
		if (achi.AnyNotDesertBlkWhoseAdjDesertLt4 && !accomplishedAchivement[0])
		{
			accomplishedAchivement[0] = true;
			index = 0;
		}
		if (achi.AnyNotDesertBlkWhoseAdjDesertEq0 && !accomplishedAchivement[1])
		{
			accomplishedAchivement[1] = true;
			index = 1;
		}
		if (achi.AnyGreenBlkWhoseAdjAllGreen && !accomplishedAchivement[2])
		{
			accomplishedAchivement[2] = true;
			index = 2;
		}
		if (achi.SeedingTriggered && !accomplishedAchivement[3])
		{
			accomplishedAchivement[3] = true;
			index = 3;
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
					index = i;
				}

			}
		}
		return index;
	}
	private void Update()
	{
		if (CheckNewRound())
		{
			NewRound();
		}
		UIManager.Instance.ChangePlantNeedNum((int)(mp[PlayerActions.ActionType.Plant] * labourForceCostCoef));
		UIManager.Instance.ChangeControlNeedNum((int)(mp[PlayerActions.ActionType.DesertHandle] * labourForceCostCoef));
	}

	bool CheckNewRound()
	{
		return labourForce <= 0;
	}
}

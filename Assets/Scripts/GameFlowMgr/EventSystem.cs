using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[ExecuteInEditMode]
public class EventSystem : SingletonMono<EventSystem>
{
	public TaskEventSettings taskEventSettings;
	public AchivementSettings achivementsSettings;
	[ShowInInspector]
	public List<TasksInfo> tasks = new List<TasksInfo>();
	[ShowInInspector]
	public List<AchivementInfo> achivements = new List<AchivementInfo>();
	protected override void OnInstanceAwake()
	{
		foreach (var task in taskEventSettings.tasks)
		{
			tasks.Add(new TasksInfo(task.taskIndex, task.targetGreen,task.targetNotDesert));
		}
		foreach (var achivement in achivementsSettings.achivements)
		{
			achivements.Add(new AchivementInfo(achivement));
		}

	}
	[Button]
	public void ReadInInfo()
	{
		tasks.Clear();
		achivements.Clear();
		foreach (var task in taskEventSettings.tasks)
		{
			tasks.Add(new TasksInfo(task.taskIndex, task.targetGreen, task.targetNotDesert));
		}
		foreach (var achivement in achivementsSettings.achivements)
		{
			achivements.Add(new AchivementInfo(achivement));
		}

	}
	[Button]
	public void SaveTasksSettings()
	{
		for (int i = 0; i < tasks.Count; i++)
		{
			taskEventSettings.tasks[i].targetGreen = tasks[i].targetGreen;
			taskEventSettings.tasks[i].taskIndex = tasks[i].taskIndex;
			taskEventSettings.tasks[i].accomplished = tasks[i].accomplished;
		}
	}
	[Button]
	public void SaveAchivementsSettings()
	{
		for (int i = 0; i < achivements.Count; i++)
		{
			achivementsSettings.achivements[i].index = achivements[i].index;
			achivementsSettings.achivements[i].CopyAchievementValue(achivements[i].requirement);
			achivementsSettings.achivements[i].achivementName = achivements[i].achivementName;
		}
	}

}
public class TasksInfo
{
	public TasksInfo(int _taskIndex, int _targetG, int _targetNotDesert, bool accomplished = false)
	{
		taskIndex = _taskIndex;
		targetGreen = _targetG;
		targetNotDesert = _targetNotDesert;
	}
	[Header("任务号")]
	public int taskIndex;
	[Header("目标绿地")]
	public int targetGreen;
	[Header("目标非沙漠化地")]
	public int targetNotDesert;
	[Header("是否达成")]
	[OnValueChanged("GetLetter")]
	public bool accomplished;
	public void GetLetter()
	{
		GameFlowCtrler.Instance.letterInfo.currentLetterCount++;
	}
}
public class AchivementInfo
{
	public AchivementInfo(Achivement achivement) {
		index = achivement.index;
		achivementName = achivement.achivementName;
		requirement.AnyNotDesertBlkWhoseAdjDesertLt4 = achivement.AnyNotDesertBlkWhoseAdjDesertLt4;
		requirement.AnyGreenBlkWhoseAdjAllGreen = achivement.AnyGreenBlkWhoseAdjAllGreen;
		requirement.AnyNotDesertBlkWhoseAdjDesertEq0 = achivement.AnyNotDesertBlkWhoseAdjDesertEq0;
		requirement.greenBlockReach = achivement.greenBlockReach;
		requirement.letterCount = achivement.letterCount;
		requirement.SeedingTriggered = achivement.SeedingTriggered;
		requirement.noDegredationRoundCount = achivement.noDegredationRoundCount;
	}
	[Header("成就序号：")]
	[OnValueChanged("RefreshName")]
	public int index;
	[Header("成就名：")]
	[ValueDropdown("achivementNames")]
	public string achivementName;
	private string[] achivementNames = new string[]
	{
		"积少成多I","积少成多II","积少成多III","牧绿者I","牧绿者II","牧绿者III","一纸心温I","一纸心温II","荒漠家书","慎重治沙者I","慎重治沙者II"
	};
	[Header("成就达成要求(详情，可关闭)")]
	public AchivementReq requirement;
	public void RefreshName()
	{
		achivementName = achivementNames[index];
	}

}


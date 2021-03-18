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
	[Header("�����")]
	public int taskIndex;
	[Header("Ŀ���̵�")]
	public int targetGreen;
	[Header("Ŀ���ɳĮ����")]
	public int targetNotDesert;
	[Header("�Ƿ���")]
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
	[Header("�ɾ���ţ�")]
	[OnValueChanged("RefreshName")]
	public int index;
	[Header("�ɾ�����")]
	[ValueDropdown("achivementNames")]
	public string achivementName;
	private string[] achivementNames = new string[]
	{
		"���ٳɶ�I","���ٳɶ�II","���ٳɶ�III","������I","������II","������III","һֽ����I","һֽ����II","��Į����","������ɳ��I","������ɳ��II"
	};
	[Header("�ɾʹ��Ҫ��(���飬�ɹر�)")]
	public AchivementReq requirement;
	public void RefreshName()
	{
		achivementName = achivementNames[index];
	}

}


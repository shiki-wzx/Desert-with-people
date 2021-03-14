using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum LocalEventProperty
{
	OnRoundEnd = 0,
	OnSpecificRoundEnd = 1,
	InRound = 2
}
[CreateAssetMenu(fileName = "Event", menuName = "CreateEvent/Event")]
public class Event : ScriptableObject
{
	[OnValueChanged("RefreshName")]
	public int index;
	public bool isRandomEvent = false;
	[InfoBox("��0��ʼ����һ�غ�Ϊ0���ڶ��غ�Ϊ1")]
	public int specificRound;
	[ValueDropdown("names")]
	public string effect;
	private string[] names = new string[]
	{
		"�ָ��Ͷ��������ޣ�����ɳ���ȡ�������",
		"��������50�Ͷ�������",
		"��ǰ�غ����ɳ������",
		"��ͼ���������ɳ��/ɳ�ر�Ϊƽ�صؿ飬�������ֲ�û�оͲ��ᴥ�����ؿ��˻�Ϊ�ݵأ���������50�Ͷ�������",
		"��ǰ�غ��Ͷ�������-40%",
		"���·���",
		"��ǰ�غ����ɳ������",
		"��ǰ�غ��Ͷ���+50",
		"��ǰ�غ��Ͷ���+100",
		"��ǰ�غ��Ͷ���-50",
		"��ǰ�غ��Ͷ�������+20%",
		"��ǰ�غ��Ͷ�������-20%",
		"��ǰ�غ��Ͷ���+50"
	};
	[Title("�İ�����")]
	public string displayContent;
	[Title("Random Event Only")]
	[InfoBox("Ȩ�أ�1/12�Ļ���Ϊ12*1/12=1��д1��1/3��д4��1/6д2")]
	public int probabilityWeight;
	[Header("����Ϊ���¼����ݿ�")]
	[InfoBox("���¼������ʣ�")]
	public LocalEventProperty property;
	[Header("�Ͷ����޸����")]
	[InfoBox("�Ͷ������ĳ������ϵ��")]
	public float labourForceCostMultiplier=1.0f;
	[InfoBox("�Ͷ������޵������ϵ��")]
	public float riseLabourForceMaximunBy;
	[InfoBox("�Ͷ�������")]
	public float labourForceIncrease;
	[InfoBox("�Ƿ�ɳ��")]
	public bool desertify = true;
	[InfoBox("Ŀǰ��document�趨��ֻ�����Ϊ3����Ҫ����Ϊtrue")]
	public bool blockRandomDegredate = false;
	private void OnEnable()
	{
		effect = names[index];
	}
	public void RefreshName()
	{
		effect = names[index];
	}
}

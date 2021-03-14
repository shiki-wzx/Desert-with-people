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
	[InfoBox("从0开始，第一回合为0，第二回合为1")]
	public int specificRound;
	[ValueDropdown("names")]
	public string effect;
	private string[] names = new string[]
	{
		"恢复劳动力至上限，结算沙化等…………",
		"永久增加50劳动力上限",
		"当前回合免除沙化结算",
		"地图上随机两处沙丘/沙地变为平地地块，随机两处植物（没有就不会触发）地块退化为草地；永久增加50劳动力上限",
		"当前回合劳动力消耗-40%",
		"无事发生",
		"当前回合免除沙化结算",
		"当前回合劳动力+50",
		"当前回合劳动力+100",
		"当前回合劳动力-50",
		"当前回合劳动力消耗+20%",
		"当前回合劳动力消耗-20%",
		"当前回合劳动力+50"
	};
	[Title("文案这里")]
	public string displayContent;
	[Title("Random Event Only")]
	[InfoBox("权重，1/12的话因为12*1/12=1就写1，1/3就写4，1/6写2")]
	public int probabilityWeight;
	[Header("以下为该事件数据库")]
	[InfoBox("该事件的性质：")]
	public LocalEventProperty property;
	[Header("劳动力修改面板")]
	[InfoBox("劳动力消耗乘以相关系数")]
	public float labourForceCostMultiplier=1.0f;
	[InfoBox("劳动力上限叠加相关系数")]
	public float riseLabourForceMaximunBy;
	[InfoBox("劳动力增加")]
	public float labourForceIncrease;
	[InfoBox("是否沙化")]
	public bool desertify = true;
	[InfoBox("目前的document设定中只有序号为3的需要此项为true")]
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

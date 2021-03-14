using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="Achivement",menuName ="CreateEvent/Achivement")]
public class Achivement : ScriptableObject
{
    [OnValueChanged("RefreshName")]
    public int index;
    [ValueDropdown("names")]
    public string achivementName;
    [InfoBox("文案读取这里的")]
    public string displayContent;
    public bool achieved = false;
    private string[] names = new string[]
    {
        "积少成多I","积少成多II","积少成多III","牧绿者I","牧绿者II","牧绿者III","一纸心温I","一纸心温II","荒漠家书","慎重治沙者I","慎重治沙者II"
    };
    [ShowInInspector]
    public AchivementReq requirements;
    private void Awake()
    {
        achivementName = names[index];
    }
    public void RefreshName()
    {
        achivementName = names[index];
    }
}
public struct AchivementReq
{
    [Header("绿色块达到：")]
    public int greenBlockReach;
    [Header("累计多少轮次没有退化")]
    public int noDegredationRoundCount;
    [Header("积少成多 I")]
    public bool AnyNotDesertBlkWhoseAdjDesertLt4;
    [Header("积少成多 II")]
    [OnValueChanged("ANDBADEq0Accomplished")]
    public bool AnyNotDesertBlkWhoseAdjDesertEq0;
    [Header("积少成多 III")]
    [OnValueChanged("AGBWAAGAccomplished")]
    public bool AnyGreenBlkWhoseAdjAllGreen;
    [Header("牧绿者 I")]
    public bool SeedingTriggered;
    [Header("家书数量")]
    public int letterCount;
    void ANDBADEq0Accomplished()
    {
        AnyNotDesertBlkWhoseAdjDesertLt4 = true;
    }
    void AGBWAAGAccomplished()
    {
        AnyNotDesertBlkWhoseAdjDesertEq0 = true;
        AnyNotDesertBlkWhoseAdjDesertLt4 = true;
    }
}

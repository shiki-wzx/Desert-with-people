using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="Achivement",menuName ="CreateEvent/Achivement")]
public class Achivement : ScriptableObject
{
    [Header("��ɫ��ﵽ��")]
    public int greenBlockReach;
    [Header("�ۼƶ����ִ�û���˻�")]
    public int noDegredationRoundCount;
    [Header("���ٳɶ� I")]
    public bool AnyNotDesertBlkWhoseAdjDesertLt4;
    [Header("���ٳɶ� II")]
    [OnValueChanged("ANDBADEq0Accomplished")]
    public bool AnyNotDesertBlkWhoseAdjDesertEq0;
    [Header("���ٳɶ� III")]
    [OnValueChanged("AGBWAAGAccomplished")]
    public bool AnyGreenBlkWhoseAdjAllGreen;
    [Header("������ I")]
    public bool SeedingTriggered;
    [Header("��������")]
    public int letterCount;
    [OnValueChanged("RefreshName")]
    public int index;
    [ValueDropdown("names")]
    public string achivementName;
    [InfoBox("�İ���ȡ�����")]
    public string displayContent;
    public bool achieved = false;
    private string[] names = new string[]
    {
        "���ٳɶ�I","���ٳɶ�II","���ٳɶ�III","������I","������II","������III","һֽ����I","һֽ����II","��Į����","������ɳ��I","������ɳ��II"
    };
    private void Awake()
    {
        achivementName = names[index];
    }
    public void RefreshName()
    {
        achivementName = names[index];
    }
    public void CopyAchievementValue(AchivementReq req)
    {
        greenBlockReach = req.greenBlockReach;
        AnyGreenBlkWhoseAdjAllGreen = req.AnyGreenBlkWhoseAdjAllGreen;
        AnyNotDesertBlkWhoseAdjDesertEq0 = req.AnyNotDesertBlkWhoseAdjDesertEq0;
        AnyNotDesertBlkWhoseAdjDesertLt4 = req.AnyNotDesertBlkWhoseAdjDesertLt4;
        SeedingTriggered = req.SeedingTriggered;
        noDegredationRoundCount = req.noDegredationRoundCount;
        letterCount = req.letterCount;
    }
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
public struct AchivementReq
{
    [Header("��ɫ��ﵽ��")]
    public int greenBlockReach;
    [Header("�ۼƶ����ִ�û���˻�")]
    public int noDegredationRoundCount;
    [Header("���ٳɶ� I")]
    public bool AnyNotDesertBlkWhoseAdjDesertLt4;
    [Header("���ٳɶ� II")]
    [OnValueChanged("ANDBADEq0Accomplished")]
    public bool AnyNotDesertBlkWhoseAdjDesertEq0;
    [Header("���ٳɶ� III")]
    [OnValueChanged("AGBWAAGAccomplished")]
    public bool AnyGreenBlkWhoseAdjAllGreen;
    [Header("������ I")]
    public bool SeedingTriggered;
    [Header("��������")]
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="Achivements",menuName ="CreateEvent/CreateAchivementsSettings")]
public class AchivementSettings : ScriptableObject
{
    [ShowInInspector]
    public List<Achivement> achivements;
}

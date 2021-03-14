using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="EventSettings",menuName ="CreateEvent/CreateEventSettings")]
public class EventSettings : ScriptableObject
{
    [ShowInInspector]
    public List<Event> settings;
}

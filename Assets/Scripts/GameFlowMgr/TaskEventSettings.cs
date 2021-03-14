using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "Task", menuName = "CreateEvent/CreateTaskSettings")]
public class TaskEventSettings : ScriptableObject
{
	[ShowInInspector]
	public List<TaskEvent> tasks;
}

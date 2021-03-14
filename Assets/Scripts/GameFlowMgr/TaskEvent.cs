using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "CreateEvent/CreateTask")]
public class TaskEvent : ScriptableObject
{
	public int taskIndex;
	public int targetGreen;
	public int targetNotDesert;
	public bool accomplished = false;
}
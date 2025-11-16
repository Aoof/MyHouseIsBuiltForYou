using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayTasks", menuName = "Scriptable Objects/DayTasks")]
public class DayTasks : ScriptableObject
{
    public List<string> tasks = new();
}
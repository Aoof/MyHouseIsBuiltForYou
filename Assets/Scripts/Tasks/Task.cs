using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Task : ScriptableObject
{
    public string taskName;
    public string objectName;
    public PointsSystem.ObjectState objectState;
}
